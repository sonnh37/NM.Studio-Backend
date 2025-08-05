using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.Services;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Queries;

public class ServiceQueryHandler :
    IRequestHandler<ServiceGetAllQuery, BusinessResult>,
    IRequestHandler<ServiceGetByIdQuery, BusinessResult>
{
    protected readonly IServiceService _serviceService;

    public ServiceQueryHandler(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    public async Task<BusinessResult> Handle(ServiceGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _serviceService.GetAll<ServiceResult>(request);
    }

    public async Task<BusinessResult> Handle(ServiceGetByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _serviceService.GetById<ServiceResult>(request);
    }
}