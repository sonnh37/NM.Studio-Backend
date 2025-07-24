using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Colors;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Handler.Commands.Base;

namespace NM.Studio.Handler.Commands;

public class ColorCommandHandler : BaseCommandHandler,
    IRequestHandler<ColorUpdateCommand, BusinessResult>,
    IRequestHandler<ColorDeleteCommand, BusinessResult>,
    IRequestHandler<ColorCreateCommand, BusinessResult>
{
    protected readonly IColorService _colorService;

    public ColorCommandHandler(IColorService colorService) : base(colorService)
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
        var msgView = await _baseService.DeleteById(request.Id, request.IsPermanent);
        return msgView;
    }

    public async Task<BusinessResult> Handle(ColorUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _baseService.CreateOrUpdate<ColorResult>(request);
        return msgView;
    }
}