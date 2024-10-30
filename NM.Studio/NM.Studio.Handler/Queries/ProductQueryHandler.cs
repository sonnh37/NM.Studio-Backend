using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.Products;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;
using MediatR;

namespace NM.Studio.Handler.Queries;

public class ProductQueryHandler :
    IRequestHandler<ProductGetAllQuery, BusinessResult>,
    IRequestHandler<ProductGetByIdQuery, BusinessResult>
{
    protected readonly IProductService _productService;

    public ProductQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<BusinessResult> Handle(ProductGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _productService.GetAll<ProductResult>(request);
    }

    public async Task<BusinessResult> Handle(ProductGetByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _productService.GetById<ProductResult>(request.Id);
    }
}