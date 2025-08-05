using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.OrderStatusHistories;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class OrderStatusHistoryCommandHandler :
    IRequestHandler<OrderStatusHistoryUpdateCommand, BusinessResult>,
    IRequestHandler<OrderStatusHistoryDeleteCommand, BusinessResult>,
    IRequestHandler<OrderStatusHistoryCreateCommand, BusinessResult>
{
    protected readonly IOrderStatusHistoryService _orderStatusHistoryService;

    public OrderStatusHistoryCommandHandler(IOrderStatusHistoryService baseService)
    {
        _orderStatusHistoryService = baseService;
    }

    public async Task<BusinessResult> Handle(OrderStatusHistoryCreateCommand request,
        CancellationToken cancellationToken)
    {
        var businessResult = await _orderStatusHistoryService.CreateOrUpdate<OrderStatusHistoryResult>(request);
        return businessResult;
    }

    public async Task<BusinessResult> Handle(OrderStatusHistoryDeleteCommand request,
        CancellationToken cancellationToken)
    {
        var businessResult = await _orderStatusHistoryService.Delete(request);
        return businessResult;
    }

    public async Task<BusinessResult> Handle(OrderStatusHistoryUpdateCommand request,
        CancellationToken cancellationToken)
    {
        var businessResult = await _orderStatusHistoryService.CreateOrUpdate<OrderStatusHistoryResult>(request);
        return businessResult;
    }
}