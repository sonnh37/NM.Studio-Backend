using MediatR;
using NM.Studio.Domain.Models.Responses;

namespace NM.Studio.Domain.CQRS.Commands.ServiceBookings;

public class ServiceBookingCancelCommand : IRequest<BusinessResult>
{
    public Guid Id { get; set; }
}