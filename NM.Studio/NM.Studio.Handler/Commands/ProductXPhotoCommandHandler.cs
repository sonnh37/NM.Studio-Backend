using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.ProductXPhotos;
using NM.Studio.Domain.Models.Responses;
using MediatR;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Handler.Commands.Base;

namespace NM.Studio.Handler.Commands;

public class ProductXPhotoCommandHandler : BaseCommandHandler,
    IRequestHandler<ProductXPhotoUpdateCommand, BusinessResult>,
    IRequestHandler<ProductXPhotoDeleteCommand, BusinessResult>,
    IRequestHandler<ProductXPhotoCreateCommand, BusinessResult>
{
    protected readonly IProductXPhotoService _productXPhotoService;

    public ProductXPhotoCommandHandler(IProductXPhotoService productXPhotoService) : base(productXPhotoService)
    {
        _productXPhotoService = productXPhotoService;
    }

    public async Task<BusinessResult> Handle(ProductXPhotoCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _productXPhotoService.CreateOrUpdate<ProductXPhotoResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ProductXPhotoDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _productXPhotoService.DeleteById(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ProductXPhotoUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _baseService.CreateOrUpdate<ProductXPhotoResult>(request);
        return msgView;
    }
}