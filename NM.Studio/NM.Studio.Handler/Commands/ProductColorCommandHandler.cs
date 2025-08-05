using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.ProductColors;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class ProductColorCommandHandler :
    IRequestHandler<ProductColorUpdateCommand, BusinessResult>,
    IRequestHandler<ProductColorDeleteCommand, BusinessResult>,
    IRequestHandler<ProductColorCreateCommand, BusinessResult>
{
    protected readonly IProductColorService ProductColorService;

    public ProductColorCommandHandler(IProductColorService productColorService)
    {
        ProductColorService = productColorService;
    }

    public async Task<BusinessResult> Handle(ProductColorCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await ProductColorService.CreateOrUpdate<ProductColorResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ProductColorDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await ProductColorService.Delete(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ProductColorUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await ProductColorService.Update<ProductColorResult>(request);
        return msgView;
    }
}