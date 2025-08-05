using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.Orders;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Queries;

public class OrderQueryHandler :
    IRequestHandler<OrderGetAllQuery, BusinessResult>,
    IRequestHandler<OrderGetByIdQuery, BusinessResult>
{
    protected readonly IOrderService _orderService;

    public OrderQueryHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<BusinessResult> Handle(OrderGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _orderService.GetAll<OrderResult>(request);
    }

    public async Task<BusinessResult> Handle(OrderGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _orderService.GetById<OrderResult>(request);
    }
}