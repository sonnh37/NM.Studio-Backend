using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.CartItems;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Queries;

public class CartItemQueryHandler :
    IRequestHandler<CartItemGetAllQuery, BusinessResult>,
    IRequestHandler<CartItemGetByIdQuery, BusinessResult>
{
    protected readonly ICartItemService _cartItemService;

    public CartItemQueryHandler(ICartItemService cartItemService)
    {
        _cartItemService = cartItemService;
    }

    public async Task<BusinessResult> Handle(CartItemGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _cartItemService.GetListByQueryAsync<CartItemResult>(request);
    }

    public async Task<BusinessResult> Handle(CartItemGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _cartItemService.GetById<CartItemResult>(request);
    }
}