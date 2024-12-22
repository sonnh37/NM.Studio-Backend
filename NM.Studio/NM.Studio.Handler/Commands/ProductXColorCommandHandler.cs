using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.ProductXColors;
using NM.Studio.Domain.Models.Responses;
using MediatR;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Handler.Commands.Base;

namespace NM.Studio.Handler.Commands;

public class ProductXColorCommandHandler : BaseCommandHandler,
    IRequestHandler<ProductXColorUpdateCommand, BusinessResult>,
    IRequestHandler<ProductXColorDeleteCommand, BusinessResult>,
    IRequestHandler<ProductXColorCreateCommand, BusinessResult>
{
    protected readonly IProductXColorService _productXColorService;

    public ProductXColorCommandHandler(IProductXColorService productXColorService) : base(productXColorService)
    {
        _productXColorService = productXColorService;
    }

    public async Task<BusinessResult> Handle(ProductXColorCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _productXColorService.CreateOrUpdate<ProductXColorResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ProductXColorDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _productXColorService.DeleteById(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ProductXColorUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _productXColorService.Update<ProductXColorResult>(request);
        return msgView;
    }
}