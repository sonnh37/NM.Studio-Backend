using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.ProductColors;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Handler.Queries;

public class ProductColorQueryHandler :
    IRequestHandler<ProductColorGetAllQuery, BusinessResult>,
    IRequestHandler<ProductColorGetByIdQuery, BusinessResult>
{
    protected readonly IProductColorService ProductColorService;

    public ProductColorQueryHandler(IProductColorService productColorService)
    {
        ProductColorService = productColorService;
    }

    public async Task<BusinessResult> Handle(ProductColorGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await ProductColorService.GetListByQueryAsync<ProductColorResult>(request);
    }

    public async Task<BusinessResult> Handle(ProductColorGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await ProductColorService.GetById<ProductColorResult>(request.Id);
    }
}