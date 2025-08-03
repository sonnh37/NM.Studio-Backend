using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.AlbumMedias;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class AlbumMediaCommandHandler :
    IRequestHandler<AlbumMediaUpdateCommand, BusinessResult>,
    IRequestHandler<AlbumMediaDeleteCommand, BusinessResult>,
    IRequestHandler<AlbumMediaCreateCommand, BusinessResult>
{
    protected readonly IAlbumMediaService _albumMediaService;

    public AlbumMediaCommandHandler(IAlbumMediaService albumMediaService)
    {
        _albumMediaService = albumMediaService;
    }

    public async Task<BusinessResult> Handle(AlbumMediaCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _albumMediaService.CreateOrUpdate<AlbumMediaResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(AlbumMediaDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _albumMediaService.DeleteById(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(AlbumMediaUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _albumMediaService.CreateOrUpdate<AlbumMediaResult>(request);
        return msgView;
    }
}