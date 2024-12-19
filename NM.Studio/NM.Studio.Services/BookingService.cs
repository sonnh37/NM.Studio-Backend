using AutoMapper;
using Microsoft.AspNetCore.Http;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Bookings;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Enums;
using NM.Studio.Domain.Models;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class BookingService : BaseService<Booking>, IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IEmailService _emailService;

    public BookingService(IMapper mapper, IEmailService emailService,
        IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        : base(mapper, unitOfWork, httpContextAccessor)
    {
        _emailService = emailService;
        _bookingRepository = _unitOfWork.BookingRepository;
        _serviceRepository = _unitOfWork.ServiceRepository;
    }

    public async Task<BusinessResult> Create<TResult>(BookingCreateCommand createCommand) where TResult : BaseResult
    {
        try
        {
            var frontendUrl = _httpContextAccessor.HttpContext?.Request.Headers.Origin.ToString();
            createCommand.Status = BookingStatus.Pending;
            var entity = await CreateOrUpdateEntity(createCommand);
            if (entity == null) return ResponseHelper.Error("Error creating booking");

            // get service
            if (entity.ServiceId == null) return ResponseHelper.Error("Error creating booking by serviceId null ");

            var service = _serviceRepository.GetById(entity.ServiceId.Value).Result;
            if (service == null) return ResponseHelper.Error("Error creating booking by service null ");

            // Gửi email
            // Tạo nội dung email dạng HTML
            var cancelLink = $"{frontendUrl}/cancel-booking/{entity.Id}"; // Link cancel booking

            var emailBody = $@"
    <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
    <h1 style='color: #4CAF50;'>Xác nhận đặt lịch</h1>
    <p>Chào {entity.FullName},</p>
    <p>Lịch hẹn của bạn đã được tạo thành công. Thông tin chi tiết:</p>
    <ul>
        <li><strong>Họ và tên:</strong> {entity.FullName}</li>
        <li><strong>Số điện thoại:</strong> {entity.Phone}</li>
        <li><strong>Dịch vụ:</strong> {service.Name}</li>
        <li><strong>Ngày đến:</strong> {entity.BookingDate:dd/MM/yyyy hh:mm tt}</li>
    </ul>
    <p>Nếu bạn muốn hủy lịch hẹn, vui lòng nhấn vào nút bên dưới:</p>
    <a href='{cancelLink}' style='background-color: #ff4c4c; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Hủy lịch hẹn</a>
    <p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!</p>
    <img src='https://firebasestorage.googleapis.com/v0/b/nhu-my-studio.appspot.com/o/1.png?alt=media&token=129af6c4-255e-455c-819f-1d83d24d605b' alt='My Studio Logo' style='margin-top: 20px; width: 150px;' />
</div>
";
            await _emailService.SendEmailAsync(entity.Email, "Booking Confirmation", emailBody);

            var result = _mapper.Map<TResult>(entity);
            var msg = ResponseHelper.Success(result);

            return msg;
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred while updating {typeof(BookingCreateCommand).Name}: {ex.Message}";
            return ResponseHelper.Error(errorMessage);
        }
    }

    public async Task<BusinessResult> Cancel<TResult>(BookingCancelCommand cancelCommand) where TResult : BaseResult
    {
        try
        {
            //check and set status cancelled
            var booking = await _bookingRepository.GetByIdNoInclude(cancelCommand.Id);
                
            if (booking == null) return ResponseHelper.NotFound();
            
            booking.Status = BookingStatus.Cancelled;
            
            _bookingRepository.Update(booking);
            var saveChanges = await _unitOfWork.SaveChanges();
            if (!saveChanges) return ResponseHelper.Error("Error saving changes");
            
            var result = _mapper.Map<TResult>(booking);
            
            if (result == null) return ResponseHelper.Warning(result);

            var msg = ResponseHelper.Success(result);

            return msg;
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred while updating {typeof(BookingCreateCommand).Name}: {ex.Message}";
            return ResponseHelper.Error(errorMessage);
        }
    }
}