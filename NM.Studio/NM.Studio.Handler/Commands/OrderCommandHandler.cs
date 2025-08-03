using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Orders;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class OrderCommandHandler :
    IRequestHandler<OrderUpdateCommand, BusinessResult>,
    IRequestHandler<OrderDeleteCommand, BusinessResult>,
    IRequestHandler<OrderCreateCommand, BusinessResult>
{
    protected readonly IOrderService _orderService;

    public OrderCommandHandler(IOrderService baseService)
    {
        _orderService = baseService;
    }

    public async Task<BusinessResult> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _orderService.CreateOrUpdate<OrderResult>(request);
        return businessResult;
    }

    public async Task<BusinessResult> Handle(OrderDeleteCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _orderService.DeleteById(request.Id, request.IsPermanent);
        return businessResult;
    }

    public async Task<BusinessResult> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _orderService.CreateOrUpdate<OrderResult>(request);
        return businessResult;
    }
}