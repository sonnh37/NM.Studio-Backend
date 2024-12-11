using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Bookings;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Handler.Commands.Base;

namespace NM.Studio.Handler.Commands;

public class BookingCommandHandler : BaseCommandHandler,
    IRequestHandler<BookingUpdateCommand, BusinessResult>,
    IRequestHandler<BookingDeleteCommand, BusinessResult>,
    IRequestHandler<BookingCreateCommand, BusinessResult>,
    IRequestHandler<BookingCancelCommand, BusinessResult>
{
    protected readonly IBookingService _bookingService;

    public BookingCommandHandler(IBookingService bookingService) : base(bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<BusinessResult> Handle(BookingCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _bookingService.Create<BookingResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(BookingDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _baseService.DeleteById(request.Id, request.IsPermanent);
        return msgView;
    }

    public async Task<BusinessResult> Handle(BookingUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _baseService.CreateOrUpdate<BookingResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(BookingCancelCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _bookingService.Cancel<BookingResult>(request);
        return msgView;
    }
}