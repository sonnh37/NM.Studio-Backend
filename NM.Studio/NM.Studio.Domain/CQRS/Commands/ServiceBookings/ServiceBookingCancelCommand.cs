using MediatR;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.CQRS.Commands.ServiceBookings;

public class ServiceBookingCancelCommand : IRequest<BusinessResult>
{
    public Guid Id { get; set; }
}