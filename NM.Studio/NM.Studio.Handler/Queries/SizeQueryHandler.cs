using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.Sizes;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

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
        return await _sizeService.GetListByQueryAsync<SizeResult>(request);
    }

    public async Task<BusinessResult> Handle(SizeGetByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _sizeService.GetById<SizeResult>(request);
    }
}