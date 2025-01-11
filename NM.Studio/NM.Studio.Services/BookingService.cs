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
            // Giả sử BookingDate gốc là giờ UTC nhưng không có Kind
            if (createCommand.BookingDate != null)
                createCommand.BookingDate = DateTime.SpecifyKind(createCommand.BookingDate.Value, DateTimeKind.Utc);

            var service = _serviceRepository.GetById(createCommand.ServiceId!.Value).Result;
            if (service == null) return new ResponseBuilder()
                .WithStatus(Const.FAIL_CODE)
                .WithMessage("Error creating booking by serviceId null")
                .Build();
            
            var entity = await CreateOrUpdateEntity(createCommand);
            if (entity == null) return new ResponseBuilder()
                .WithStatus(Const.FAIL_CODE)
                .WithMessage(Const.FAIL_SAVE_MSG)
                .Build();

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

            return new ResponseBuilder<TResult>()
                .WithData(result)
                .WithStatus(Const.SUCCESS_CODE)
                .WithMessage("Send email successfully.")
                .Build();
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred while updating {typeof(BookingCreateCommand).Name}: {ex.Message}";
            return new ResponseBuilder()
                .WithStatus(Const.FAIL_CODE)
                .WithMessage(errorMessage)
                .Build();
        }
    }

    public async Task<BusinessResult> Cancel<TResult>(BookingCancelCommand cancelCommand) where TResult : BaseResult
    {
        try
        {
            //check and set status cancelled
            var booking = await _bookingRepository.GetById(cancelCommand.Id);
                
            if (booking == null) return new ResponseBuilder()
                .WithStatus(Const.NOT_FOUND_CODE)
                .WithMessage(Const.NOT_FOUND_MSG)
                .Build();
            booking.Status = BookingStatus.Cancelled;
            
            _bookingRepository.Update(booking);
            var saveChanges = await _unitOfWork.SaveChanges();
            if (!saveChanges) return new ResponseBuilder()
                .WithStatus(Const.FAIL_CODE)
                .WithMessage(Const.FAIL_SAVE_MSG)
                .Build();
            var result = _mapper.Map<TResult>(booking);
            
            return new ResponseBuilder<TResult>()
                .WithData(result)
                .WithStatus(Const.SUCCESS_CODE)
                .WithMessage(Const.SUCCESS_SAVE_MSG)
                .Build();
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred while updating {typeof(BookingCreateCommand).Name}: {ex.Message}";
            return new ResponseBuilder()
                .WithStatus(Const.FAIL_CODE)
                .WithMessage(errorMessage)
                .Build();
        }
    }
}