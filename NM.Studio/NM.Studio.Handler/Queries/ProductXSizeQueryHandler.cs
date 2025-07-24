using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.ProductSizes;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Handler.Queries;

public class ProductSizeQueryHandler :
    IRequestHandler<ProductSizeGetAllQuery, BusinessResult>,
    IRequestHandler<ProductSizeGetByIdQuery, BusinessResult>
{
    protected readonly IProductSizeService ProductSizeService;

    public ProductSizeQueryHandler(IProductSizeService productSizeService)
    {
        ProductSizeService = productSizeService;
    }

    public async Task<BusinessResult> Handle(ProductSizeGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await ProductSizeService.GetListByQueryAsync<ProductSizeResult>(request);
    }

    public async Task<BusinessResult> Handle(ProductSizeGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await ProductSizeService.GetById<ProductSizeResult>(request.Id);
    }
}