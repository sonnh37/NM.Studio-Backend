using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;
using MediatR;
using NM.Studio.Domain.CQRS.Queries.Categories;
using NM.Studio.Domain.CQRS.Queries.Colors;

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
        return await _colorService.GetAll<ColorResult>(request);
    }

    public async Task<BusinessResult> Handle(ColorGetByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _colorService.GetById<ColorResult>(request.Id);
    }
}