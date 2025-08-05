using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.OrderItems;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Queries;

public class OrderItemQueryHandler :
    IRequestHandler<OrderItemGetAllQuery, BusinessResult>,
    IRequestHandler<OrderItemGetByIdQuery, BusinessResult>
{
    protected readonly IOrderItemService _orderItemService;

    public OrderItemQueryHandler(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    public async Task<BusinessResult> Handle(OrderItemGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _orderItemService.GetAll<OrderItemResult>(request);
    }

    public async Task<BusinessResult> Handle(OrderItemGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _orderItemService.GetById<OrderItemResult>(request);
    }
}