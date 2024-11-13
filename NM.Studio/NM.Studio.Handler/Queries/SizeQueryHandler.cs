using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;
using MediatR;
using NM.Studio.Domain.CQRS.Queries.Categories;
using NM.Studio.Domain.CQRS.Queries.Sizes;

namespace NM.Studio.Handler.Queries;

public class SizeQueryHandler :
    IRequestHandler<SizeGetAllQuery, BusinessResult>,
    IRequestHandler<SizeGetByIdQuery, BusinessResult>
{
    protected readonly ISizeService _sizeService;

    public SizeQueryHandler(ISizeService sizeService)
    {
        _sizeService = sizeService;
    }

    public async Task<BusinessResult> Handle(SizeGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _sizeService.GetAll<SizeResult>(request);
    }

    public async Task<BusinessResult> Handle(SizeGetByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _sizeService.GetById<SizeResult>(request.Id);
    }
}