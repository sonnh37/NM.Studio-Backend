using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.ProductXColors;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Handler.Queries;

public class ProductXColorQueryHandler :
    IRequestHandler<ProductXColorGetAllQuery, BusinessResult>,
    IRequestHandler<ProductXColorGetByIdQuery, BusinessResult>
{
    protected readonly IProductXColorService _productXColorService;

    public ProductXColorQueryHandler(IProductXColorService productXColorService)
    {
        _productXColorService = productXColorService;
    }

    public async Task<BusinessResult> Handle(ProductXColorGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _productXColorService.GetAll<ProductXColorResult>(request);
    }

    public async Task<BusinessResult> Handle(ProductXColorGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _productXColorService.GetById<ProductXColorResult>(request.Id);
    }
}