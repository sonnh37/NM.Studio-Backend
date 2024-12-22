using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.ProductXSizes;
using NM.Studio.Domain.Models.Responses;
using MediatR;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Handler.Commands.Base;

namespace NM.Studio.Handler.Commands;

public class ProductXSizeCommandHandler : BaseCommandHandler,
    IRequestHandler<ProductXSizeUpdateCommand, BusinessResult>,
    IRequestHandler<ProductXSizeDeleteCommand, BusinessResult>,
    IRequestHandler<ProductXSizeCreateCommand, BusinessResult>
{
    protected readonly IProductXSizeService _productXSizeService;

    public ProductXSizeCommandHandler(IProductXSizeService productXSizeService) : base(productXSizeService)
    {
        _productXSizeService = productXSizeService;
    }

    public async Task<BusinessResult> Handle(ProductXSizeCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _productXSizeService.CreateOrUpdate<ProductXSizeResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ProductXSizeDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _productXSizeService.DeleteById(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ProductXSizeUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _productXSizeService.Update<ProductXSizeResult>(request);
        return msgView;
    }
}