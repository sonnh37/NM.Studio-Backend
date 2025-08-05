using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.Carts;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Queries;

public class CartQueryHandler :
    IRequestHandler<CartGetAllQuery, BusinessResult>,
    IRequestHandler<CartGetByIdQuery, BusinessResult>
{
    protected readonly ICartService _cartService;

    public CartQueryHandler(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<BusinessResult> Handle(CartGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _cartService.GetAll<CartResult>(request);
    }

    public async Task<BusinessResult> Handle(CartGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _cartService.GetById<CartResult>(request);
    }
}