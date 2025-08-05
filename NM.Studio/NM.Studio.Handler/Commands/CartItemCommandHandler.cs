using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.CartItems;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class CartItemCommandHandler :
    IRequestHandler<CartItemUpdateCommand, BusinessResult>,
    IRequestHandler<CartItemDeleteCommand, BusinessResult>,
    IRequestHandler<CartItemCreateCommand, BusinessResult>
{
    protected readonly ICartItemService _cartItemService;

    public CartItemCommandHandler(ICartItemService baseService)
    {
        _cartItemService = baseService;
    }

    public async Task<BusinessResult> Handle(CartItemCreateCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _cartItemService.CreateOrUpdate<CartItemResult>(request);
        return businessResult;
    }

    public async Task<BusinessResult> Handle(CartItemDeleteCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _cartItemService.Delete(request);
        return businessResult;
    }

    public async Task<BusinessResult> Handle(CartItemUpdateCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _cartItemService.CreateOrUpdate<CartItemResult>(request);
        return businessResult;
    }
}