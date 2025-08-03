using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.OrderStatusHistories;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Queries;

public class OrderStatusHistoryQueryHandler :
    IRequestHandler<OrderStatusHistoryGetAllQuery, BusinessResult>,
    IRequestHandler<OrderStatusHistoryGetByIdQuery, BusinessResult>
{
    protected readonly IOrderStatusHistoryService _orderStatusHistoryService;

    public OrderStatusHistoryQueryHandler(IOrderStatusHistoryService orderStatusHistoryService)
    {
        _orderStatusHistoryService = orderStatusHistoryService;
    }

    public async Task<BusinessResult> Handle(OrderStatusHistoryGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _orderStatusHistoryService.GetListByQueryAsync<OrderStatusHistoryResult>(request);
    }

    public async Task<BusinessResult> Handle(OrderStatusHistoryGetByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _orderStatusHistoryService.GetById<OrderStatusHistoryResult>(request);
    }
}