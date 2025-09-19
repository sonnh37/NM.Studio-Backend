using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.CQRS.Commands.ServiceBookings;
using NM.Studio.Domain.CQRS.Queries.ServiceBookings;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;
using NM.Studio.Domain.Utilities.Filters;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class ServiceBookingService : BaseService, IServiceBookingService
{
    private readonly IEmailService _emailService;
    private readonly IServiceBookingRepository _serviceBookingRepository;
    private readonly IServiceRepository _serviceRepository;

    public ServiceBookingService(IMapper mapper, IEmailService emailService,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _emailService = emailService;
        _serviceBookingRepository = _unitOfWork.ServiceBookingRepository;
        _serviceRepository = _unitOfWork.ServiceRepository;
    }

    public async Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand)
    {
        ServiceBooking? entity = null;
        if (createOrUpdateCommand is ServiceBookingUpdateCommand updateCommand)
        {
            entity = await _serviceBookingRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefaultAsync();
            if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
            _mapper.Map(updateCommand, entity);
            _serviceBookingRepository.Update(entity);
        }
        else if (createOrUpdateCommand is ServiceBookingCreateCommand createCommand)
        {
            var frontendUrl = _httpContextAccessor.HttpContext?.Request.Headers.Origin.ToString();
            createCommand.Status = ServiceBookingStatus.Pending;
            // Giả sử BookingDate gốc là giờ UTC nhưng không có Kind

            var service = await _serviceRepository.GetQueryable(m => m.Id == createCommand.ServiceId)
                .SingleOrDefaultAsync();
            if (service == null)
                throw new DomainException("Error creating booking by serviceId null");

            // Gửi email
            // Tạo nội dung email dạng HTML
            var cancelLink = $"{frontendUrl}/cancel-booking/{service.Id}"; // Link cancel booking

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
            <p style='font-size:18px;margin-bottom:4px;'>Xin chào <b>{createCommand.CustomerName}</b>,</p>
            <p style='margin-top:0;margin-bottom:16px;'>Cảm ơn bạn đã đặt lịch với chúng tôi.<br/>Sau đây là thông tin chi tiết lịch hẹn của bạn:</p>
          </td>
        </tr>
        <tr>
          <td style='padding:0 32px 16px 32px;'>
            <table cellpadding='0' cellspacing='0' border='0' style='width:100%;margin-bottom:14px;'>
              <tr>
                <td style='font-weight:bold;width:150px;padding:8px 0;'>Họ & tên:</td>
                <td style='padding:8px 0;border-bottom:1px solid #f0f0f3;'>{createCommand.CustomerName}</td>
              </tr>
              <tr>
                <td style='font-weight:bold;padding:8px 0;'>Số điện thoại:</td>
                <td style='padding:8px 0;border-bottom:1px solid #f0f0f3;'>{createCommand.CustomerPhone}</td>
              </tr>
              <tr>
                <td style='font-weight:bold;padding:8px 0;'>Dịch vụ:</td>
                <td style='padding:8px 0;border-bottom:1px solid #f0f0f3;'>{service.Name}</td>
              </tr>
              <tr>
                <td style='font-weight:bold;padding:8px 0;'>Ngày đến:</td>
                <td style='padding:8px 0;'>
                  {createCommand.AppointmentDate:dd/MM/yyyy HH:mm} 
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
            await _emailService.SendEmailAsync(createCommand.CustomerEmail, "Booking Confirmation", emailBody);

            entity = _mapper.Map<ServiceBooking>(createCommand);
            if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
            entity.CreatedDate = DateTimeOffset.UtcNow;
            _serviceBookingRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<ServiceBookingResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> Cancel(ServiceBookingCancelCommand cancelCommand)

    {
        //check and set status cancelled
        var booking = await _serviceBookingRepository.GetQueryable(m => m.Id == cancelCommand.Id)
            .SingleOrDefaultAsync();

        if (booking == null)
            throw new NotFoundException(Const.NOT_FOUND_MSG);
        booking.Status = ServiceBookingStatus.Cancelled;

        _serviceBookingRepository.Update(booking);
        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();
        var result = _mapper.Map<ServiceBookingResult>(booking);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> GetAll(ServiceBookingGetAllQuery query)
    {
        var queryable = _serviceBookingRepository.GetQueryable();

        queryable = FilterHelper.BaseEntity(queryable, query);
        queryable = RepoHelper.Include(queryable, query.IncludeProperties);
        queryable = RepoHelper.Sort(queryable, query);

        var totalCount = await queryable.CountAsync();
        var entities = await RepoHelper.GetQueryablePagination(queryable, query).ToListAsync();
        var results = _mapper.Map<List<ServiceBookingResult>>(entities);
        var getQueryableResult = new GetQueryableResult(results, totalCount, query);

        return new BusinessResult(getQueryableResult);
    }

    public async Task<BusinessResult> GetById(ServiceBookingGetByIdQuery request)
    {
        var queryable = _serviceBookingRepository.GetQueryable(x => x.Id == request.Id);
        queryable = RepoHelper.Include(queryable, request.IncludeProperties);
        var entity = await queryable.SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException("Not found");
        var result = _mapper.Map<ServiceBookingResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> Delete(ServiceBookingDeleteCommand command)
    {
        var entity = await _serviceBookingRepository.GetQueryable(x => x.Id == command.Id).SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);

        _serviceBookingRepository.Delete(entity, command.IsPermanent);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        return new BusinessResult();
    }
}