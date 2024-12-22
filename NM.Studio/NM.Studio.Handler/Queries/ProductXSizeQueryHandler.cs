using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.ProductXSizes;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Handler.Queries;

public class ProductXSizeQueryHandler :
    IRequestHandler<ProductXSizeGetAllQuery, BusinessResult>,
    IRequestHandler<ProductXSizeGetByIdQuery, BusinessResult>
{
    protected readonly IProductXSizeService _productXSizeService;

    public ProductXSizeQueryHandler(IProductXSizeService productXSizeService)
    {
        _productXSizeService = productXSizeService;
    }

    public async Task<BusinessResult> Handle(ProductXSizeGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _productXSizeService.GetAll<ProductXSizeResult>(request);
    }

    public async Task<BusinessResult> Handle(ProductXSizeGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _productXSizeService.GetById<ProductXSizeResult>(request.Id);
    }
}