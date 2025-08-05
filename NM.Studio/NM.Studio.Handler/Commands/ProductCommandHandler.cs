using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Products;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class ProductCommandHandler :
    IRequestHandler<ProductCreateCommand, BusinessResult>,
    IRequestHandler<ProductUpdateCommand, BusinessResult>,
    IRequestHandler<ProductDeleteCommand, BusinessResult>
{
    protected readonly IProductService _productService;

    public ProductCommandHandler(IProductService serviceProduct) 
    {
        _productService = serviceProduct;
    }

    public async Task<BusinessResult> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _productService.Create<ProductResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _productService.Delete(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _productService.Update<ProductResult>(request);
        return msgView;
    }
}