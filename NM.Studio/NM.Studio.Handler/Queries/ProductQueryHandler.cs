using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.Products;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Queries;

public class ProductQueryHandler :
    IRequestHandler<ProductGetAllQuery, BusinessResult>,
    IRequestHandler<ProductGetByIdQuery, BusinessResult>,
    IRequestHandler<ProductRepresentativeByCategoryQuery, BusinessResult>
{
    protected readonly IProductService _productService;

    public ProductQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<BusinessResult> Handle(ProductGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _productService.GetListByQueryAsync<ProductResult>(request);
    }

    public async Task<BusinessResult> Handle(ProductGetByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _productService.GetById<ProductResult>(request);
    }

    public async Task<BusinessResult> Handle(ProductRepresentativeByCategoryQuery request,
        CancellationToken cancellationToken)
    {
        return await _productService.GetRepresentativeByCategory(request);
    }
}