using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.ProductMedias;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class ProductMediaCommandHandler :
    IRequestHandler<ProductMediaUpdateCommand, BusinessResult>,
    IRequestHandler<ProductMediaDeleteCommand, BusinessResult>,
    IRequestHandler<ProductMediaCreateCommand, BusinessResult>
{
    protected readonly IProductMediaService _productMediaService;

    public ProductMediaCommandHandler(IProductMediaService productMediaService) 
    {
        _productMediaService = productMediaService;
    }

    public async Task<BusinessResult> Handle(ProductMediaCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _productMediaService.CreateOrUpdate<ProductMediaResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ProductMediaDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _productMediaService.Delete(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ProductMediaUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _productMediaService.CreateOrUpdate<ProductMediaResult>(request);
        return msgView;
    }
}