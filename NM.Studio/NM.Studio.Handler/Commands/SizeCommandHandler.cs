using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.Responses;
using MediatR;
using NM.Studio.Domain.CQRS.Commands.Categories;
using NM.Studio.Domain.CQRS.Commands.Sizes;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Handler.Commands.Base;

namespace NM.Studio.Handler.Commands;

public class SizeCommandHandler : BaseCommandHandler,
    IRequestHandler<SizeUpdateCommand, BusinessResult>,
    IRequestHandler<SizeDeleteCommand, BusinessResult>,
    IRequestHandler<SizeCreateCommand, BusinessResult>
{
    protected readonly ISizeService _sizeService;

    public SizeCommandHandler(ISizeService sizeService) : base(sizeService)
    {
        _sizeService = sizeService;
    }

    public async Task<BusinessResult> Handle(SizeCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _sizeService.CreateOrUpdate<SizeResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(SizeDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _baseService.DeleteById(request.Id, request.IsPermanent);
        return msgView;
    }

    public async Task<BusinessResult> Handle(SizeUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _baseService.CreateOrUpdate<SizeResult>(request);
        return msgView;
    }
}