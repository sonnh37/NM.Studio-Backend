using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Albums;
using NM.Studio.Domain.Models.Responses;
using MediatR;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Handler.Commands.Base;

namespace NM.Studio.Handler.Commands;

public class AlbumCommandHandler : BaseCommandHandler,
    IRequestHandler<AlbumUpdateCommand, BusinessResult>,
    IRequestHandler<AlbumDeleteCommand, BusinessResult>,
    IRequestHandler<AlbumCreateCommand, BusinessResult>
{
    protected readonly IAlbumService _albumService;

    public AlbumCommandHandler(IAlbumService albumService) : base(albumService)
    {
        _albumService = albumService;
    }

    public async Task<BusinessResult> Handle(AlbumCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _albumService.Create<AlbumResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(AlbumDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _albumService.DeleteById(request.Id, request.IsPermanent);
        return msgView;
    }

    public async Task<BusinessResult> Handle(AlbumUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _albumService.Update<AlbumResult>(request);
        return msgView;
    }
}