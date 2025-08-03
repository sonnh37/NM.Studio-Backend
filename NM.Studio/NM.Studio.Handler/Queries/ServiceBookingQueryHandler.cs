using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.ServiceBookings;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Queries;

public class ServiceBookingQueryHandler :
    IRequestHandler<ServiceBookingGetAllQuery, BusinessResult>,
    IRequestHandler<ServiceBookingGetByIdQuery, BusinessResult>
{
    protected readonly IServiceBookingService ServiceBookingService;

    public ServiceBookingQueryHandler(IServiceBookingService serviceBookingService)
    {
        ServiceBookingService = serviceBookingService;
    }

    public async Task<BusinessResult> Handle(ServiceBookingGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await ServiceBookingService.GetListByQueryAsync<ServiceBookingResult>(request);
    }

    public async Task<BusinessResult> Handle(ServiceBookingGetByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await ServiceBookingService.GetById<ServiceBookingResult>(request);
    }
}