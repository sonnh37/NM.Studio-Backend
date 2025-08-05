using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Colors;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class ColorCommandHandler :
    IRequestHandler<ColorUpdateCommand, BusinessResult>,
    IRequestHandler<ColorDeleteCommand, BusinessResult>,
    IRequestHandler<ColorCreateCommand, BusinessResult>
{
    protected readonly IColorService _colorService;

    public ColorCommandHandler(IColorService colorService)
    {
        _colorService = colorService;
    }

    public async Task<BusinessResult> Handle(ColorCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _colorService.CreateOrUpdate<ColorResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ColorDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _colorService.Delete(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ColorUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _colorService.CreateOrUpdate<ColorResult>(request);
        return msgView;
    }
}