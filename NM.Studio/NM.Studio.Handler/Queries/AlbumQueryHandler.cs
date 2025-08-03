using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.Albums;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Queries;

public class AlbumQueryHandler :
    IRequestHandler<AlbumGetAllQuery, BusinessResult>,
    IRequestHandler<AlbumGetByIdQuery, BusinessResult>
{
    protected readonly IAlbumService _albumService;

    public AlbumQueryHandler(IAlbumService albumService)
    {
        _albumService = albumService;
    }

    public async Task<BusinessResult> Handle(AlbumGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _albumService.GetListByQueryAsync<AlbumResult>(request);
    }

    public async Task<BusinessResult> Handle(AlbumGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _albumService.GetById<AlbumResult>(request);
    }
}