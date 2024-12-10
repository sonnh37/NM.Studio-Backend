using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.Bookings;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Handler.Queries;

public class BookingQueryHandler :
    IRequestHandler<BookingGetAllQuery, BusinessResult>,
    IRequestHandler<BookingGetByIdQuery, BusinessResult>
{
    protected readonly IBookingService _bookingService;

    public BookingQueryHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<BusinessResult> Handle(BookingGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _bookingService.GetAll<BookingResult>(request);
    }

    public async Task<BusinessResult> Handle(BookingGetByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _bookingService.GetById<BookingResult>(request.Id);
    }
}