using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.OrderItems;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class OrderItemCommandHandler :
    IRequestHandler<OrderItemUpdateCommand, BusinessResult>,
    IRequestHandler<OrderItemDeleteCommand, BusinessResult>,
    IRequestHandler<OrderItemCreateCommand, BusinessResult>
{
    protected readonly IOrderItemService _orderItemService;

    public OrderItemCommandHandler(IOrderItemService baseService)
    {
        _orderItemService = baseService;
    }

    public async Task<BusinessResult> Handle(OrderItemCreateCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _orderItemService.CreateOrUpdate<OrderItemResult>(request);
        return businessResult;
    }

    public async Task<BusinessResult> Handle(OrderItemDeleteCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _orderItemService.DeleteById(request.Id, request.IsPermanent);
        return businessResult;
    }

    public async Task<BusinessResult> Handle(OrderItemUpdateCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _orderItemService.CreateOrUpdate<OrderItemResult>(request);
        return businessResult;
    }
}