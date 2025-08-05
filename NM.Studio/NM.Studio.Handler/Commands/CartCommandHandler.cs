using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Carts;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class CartCommandHandler :
    IRequestHandler<CartUpdateCommand, BusinessResult>,
    IRequestHandler<CartDeleteCommand, BusinessResult>,
    IRequestHandler<CartCreateCommand, BusinessResult>
{
    protected readonly ICartService _cartService;

    public CartCommandHandler(ICartService baseService)
    {
        _cartService = baseService;
    }

    public async Task<BusinessResult> Handle(CartCreateCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _cartService.CreateOrUpdate<CartResult>(request);
        return businessResult;
    }

    public async Task<BusinessResult> Handle(CartDeleteCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _cartService.Delete(request);
        return businessResult;
    }

    public async Task<BusinessResult> Handle(CartUpdateCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _cartService.CreateOrUpdate<CartResult>(request);
        return businessResult;
    }
}