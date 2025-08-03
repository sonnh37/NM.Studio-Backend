using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Albums;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class AlbumCommandHandler : IRequestHandler<AlbumUpdateCommand, BusinessResult>,
    IRequestHandler<AlbumDeleteCommand, BusinessResult>,
    IRequestHandler<AlbumCreateCommand, BusinessResult>
{
    protected readonly IAlbumService _albumService;

    public AlbumCommandHandler(IAlbumService albumService)
    {
        _albumService = albumService;
    }

    public async Task<BusinessResult> Handle(AlbumCreateCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _albumService.Create<AlbumResult>(request);
        return businessResult;
    }

    public async Task<BusinessResult> Handle(AlbumDeleteCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _albumService.DeleteById(request.Id, request.IsPermanent);
        return businessResult;
    }


    public async Task<BusinessResult> Handle(AlbumUpdateCommand request, CancellationToken cancellationToken)
    {
        var businessResult = await _albumService.Update<AlbumResult>(request);
        return businessResult;
    }
}