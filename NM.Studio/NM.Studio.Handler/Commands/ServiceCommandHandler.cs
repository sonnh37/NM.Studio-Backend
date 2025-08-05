using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Services;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class ServiceCommandHandler :
    IRequestHandler<ServiceUpdateCommand, BusinessResult>,
    IRequestHandler<ServiceDeleteCommand, BusinessResult>,
    IRequestHandler<ServiceCreateCommand, BusinessResult>
{
    private readonly IServiceService _serviceService;

    public ServiceCommandHandler(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    public async Task<BusinessResult> Handle(ServiceCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _serviceService.Create<ServiceResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ServiceDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _serviceService.Delete(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ServiceUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _serviceService.Update<ServiceResult>(request);
        return msgView;
    }
}