using AutoMapper;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.ServiceBookings;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Enums;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class ServiceBookingService : BaseService<ServiceBooking>, IServiceBookingService
{
    private readonly IServiceBookingRepository _serviceBookingRepository;
    private readonly IEmailService _emailService;
    private readonly IServiceRepository _serviceRepository;

    public ServiceBookingService(IMapper mapper, IEmailService emailService,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _emailService = emailService;
        _serviceBookingRepository = _unitOfWork.ServiceBookingRepository;
        _serviceRepository = _unitOfWork.ServiceRepository;
    }

    public async Task<BusinessResult> Create<TResult>(ServiceBookingCreateCommand createCommand)
        where TResult : BaseResult
    {
        try
        {
            var frontendUrl = _httpContextAccessor.HttpContext?.Request.Headers.Origin.ToString();
            createCommand.Status = ServiceBookingStatus.Pending;
            // Giả sử BookingDate gốc là giờ UTC nhưng không có Kind

            var service = _serviceRepository.GetById(createCommand.ServiceId!.Value).Result;
            if (service == null)
                return BusinessResult.Fail("Error creating booking by serviceId null");

            var entity = await CreateOrUpdateEntity(createCommand);
            if (entity == null)
                return BusinessResult.Fail();

            // Gửi email
            // Tạo nội dung email dạng HTML
            var cancelLink = $"{frontendUrl}/cancel-booking/{entity.Id}"; // Link cancel booking

            var emailBody = $@"
<table width='100%' cellpadding='0' cellspacing='0' border='0' style='background:#f7f7f9;padding:0;margin:0;'>
  <tr>
    <td align='center'>
      <table width='600' cellpadding='0' cellspacing='0' border='0' style='background:#fff;border-radius:10px; box-shadow:0 2px 10px rgba(60,60,60,0.07);margin:32px 0;font-family:Segoe UI, Arial, sans-serif;color:#222;line-height:1.6;'>
        <tr>
          <td style='padding:24px 32px 16px 32px;'>
            <h2 style='margin:0;font-size:32px;color:#1976d2;font-weight:bold;text-align:center;letter-spacing:1px;'>Xác nhận đặt lịch</h2>
          </td>
        </tr>
        <tr>
          <td style='padding:0 32px 12px 32px;'>
            <p style='font-size:18px;margin-bottom:4px;'>Xin chào <b>{entity.CustomerName}</b>,</p>
            <p style='margin-top:0;margin-bottom:16px;'>Cảm ơn bạn đã đặt lịch với chúng tôi.<br/>Sau đây là thông tin chi tiết lịch hẹn của bạn:</p>
          </td>
        </tr>
        <tr>
          <td style='padding:0 32px 16px 32px;'>
            <table cellpadding='0' cellspacing='0' border='0' style='width:100%;margin-bottom:14px;'>
              <tr>
                <td style='font-weight:bold;width:150px;padding:8px 0;'>Họ & tên:</td>
                <td style='padding:8px 0;border-bottom:1px solid #f0f0f3;'>{entity.CustomerName}</td>
              </tr>
              <tr>
                <td style='font-weight:bold;padding:8px 0;'>Số điện thoại:</td>
                <td style='padding:8px 0;border-bottom:1px solid #f0f0f3;'>{entity.CustomerPhone}</td>
              </tr>
              <tr>
                <td style='font-weight:bold;padding:8px 0;'>Dịch vụ:</td>
                <td style='padding:8px 0;border-bottom:1px solid #f0f0f3;'>{service.Name}</td>
              </tr>
              <tr>
                <td style='font-weight:bold;padding:8px 0;'>Ngày đến:</td>
                <td style='padding:8px 0;'>
                  {entity.AppointmentDate:dd/MM/yyyy HH:mm} 
                </td>
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td align='center' style='padding-bottom:16px;'>
            <a href='{cancelLink}' style='background:#e53935;color:#fff;padding:13px 38px;font-size:18px;border-radius:6px;text-decoration:none;display:inline-block;font-weight:bold;box-shadow:0 2px 8px #f4433650;'>HỦY LỊCH HẸN</a>
          </td>
        </tr>
        <tr>
          <td align='center' style='padding:0 32px 20px 32px;'>
            <p style='margin:0;font-size:15px;color:#444;'>Nếu bạn không thực hiện hành động này, vui lòng bỏ qua email này.<br/>
             Cảm ơn bạn đã tin tưởng và sử dụng dịch vụ của chúng tôi!</p>
          </td>
        </tr>
        <tr>
          <td align='center' style='padding-bottom:30px;'>
            <img src='https://firebasestorage.googleapis.com/v0/b/nhu-my-studio.appspot.com/o/1.png?alt=media&token=129af6c4-255e-455c-819f-1d83d24d605b'
             alt='My Studio Logo' width='130' style='margin-top:5px;border-radius:4px;'/>
          </td>
        </tr>
      </table>
    </td>
  </tr>
</table>
";
            await _emailService.SendEmailAsync(entity.CustomerEmail, "Booking Confirmation", emailBody);

            var result = _mapper.Map<TResult>(entity);

            return BusinessResult.Success(result, "Send email successfully.");
        }
        catch (Exception ex)
        {
            return BusinessResult.ExceptionError(ex.Message);
        }
    }

    public async Task<BusinessResult> Cancel<TResult>(ServiceBookingCancelCommand cancelCommand) where TResult : BaseResult
    {
        try
        {
            //check and set status cancelled
            var booking = await _serviceBookingRepository.GetById(cancelCommand.Id);

            if (booking == null)
                return BusinessResult.Fail(Const.NOT_FOUND_MSG);
            booking.Status = ServiceBookingStatus.Cancelled;

            _serviceBookingRepository.Update(booking);
            var saveChanges = await _unitOfWork.SaveChanges();
            if (!saveChanges)
                return BusinessResult.Fail();
            var result = _mapper.Map<TResult>(booking);

            return BusinessResult.Success(result);
        }
        catch (Exception ex)
        {
            return BusinessResult.ExceptionError(ex.Message);
        }
    }
}