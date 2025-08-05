using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.ProductSizes;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class ProductSizeCommandHandler :
    IRequestHandler<ProductSizeUpdateCommand, BusinessResult>,
    IRequestHandler<ProductSizeDeleteCommand, BusinessResult>,
    IRequestHandler<ProductSizeCreateCommand, BusinessResult>
{
    protected readonly IProductSizeService ProductSizeService;

    public ProductSizeCommandHandler(IProductSizeService productSizeService)
    {
        ProductSizeService = productSizeService;
    }

    public async Task<BusinessResult> Handle(ProductSizeCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await ProductSizeService.CreateOrUpdate<ProductSizeResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ProductSizeDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await ProductSizeService.Delete(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ProductSizeUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await ProductSizeService.Update<ProductSizeResult>(request);
        return msgView;
    }
}