using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Products;
using NM.Studio.Domain.Models.Responses;
using MediatR;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Handler.Commands.Base;

namespace NM.Studio.Handler.Commands;

public class ProductCommandHandler : BaseCommandHandler,
    IRequestHandler<ProductCreateCommand, BusinessResult>,
    IRequestHandler<ProductUpdateCommand, BusinessResult>,
    IRequestHandler<ProductDeleteCommand, BusinessResult>
{
    protected readonly IProductService _productService;

    public ProductCommandHandler(IProductService serviceProduct) : base(serviceProduct)
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
        var msgView = await _productService.DeleteById(request.Id);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _productService.Update<ProductResult>(request);
        return msgView;
    }
}