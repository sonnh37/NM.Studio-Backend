using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.ServiceBookings;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class ServiceBookingCommandHandler :
    IRequestHandler<ServiceBookingUpdateCommand, BusinessResult>,
    IRequestHandler<ServiceBookingDeleteCommand, BusinessResult>,
    IRequestHandler<ServiceBookingCreateCommand, BusinessResult>,
    IRequestHandler<ServiceBookingCancelCommand, BusinessResult>
{
    protected readonly IServiceBookingService ServiceBookingService;

    public ServiceBookingCommandHandler(IServiceBookingService serviceBookingService)
    {
        ServiceBookingService = serviceBookingService;
    }

    public async Task<BusinessResult> Handle(ServiceBookingCancelCommand request, CancellationToken cancellationToken)
    {
        var msgView = await ServiceBookingService.Cancel<ServiceBookingResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ServiceBookingCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await ServiceBookingService.Create<ServiceBookingResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ServiceBookingDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await ServiceBookingService.DeleteById(request.Id, request.IsPermanent);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ServiceBookingUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await ServiceBookingService.CreateOrUpdate<ServiceBookingResult>(request);
        return msgView;
    }
}