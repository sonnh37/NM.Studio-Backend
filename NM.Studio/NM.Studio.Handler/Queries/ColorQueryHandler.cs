using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.Colors;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Queries;

public class ColorQueryHandler :
    IRequestHandler<ColorGetAllQuery, BusinessResult>,
    IRequestHandler<ColorGetByIdQuery, BusinessResult>
{
    protected readonly IColorService _colorService;

    public ColorQueryHandler(IColorService colorService)
    {
        _colorService = colorService;
    }

    public async Task<BusinessResult> Handle(ColorGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _colorService.GetListByQueryAsync<ColorResult>(request);
    }

    public async Task<BusinessResult> Handle(ColorGetByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _colorService.GetById<ColorResult>(request);
    }
}