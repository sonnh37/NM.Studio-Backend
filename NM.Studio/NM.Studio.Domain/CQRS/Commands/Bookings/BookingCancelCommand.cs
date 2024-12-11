using MediatR;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.Models.Responses;

namespace NM.Studio.Domain.CQRS.Commands.Bookings;

public class BookingCancelCommand : IRequest<BusinessResult>
{
    public Guid Id { get; set; }
}