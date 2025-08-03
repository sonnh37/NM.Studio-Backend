using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.AlbumMedias;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Queries;

public class AlbumMediaQueryHandler :
    IRequestHandler<AlbumMediaGetAllQuery, BusinessResult>,
    IRequestHandler<AlbumMediaGetByIdQuery, BusinessResult>
{
    protected readonly IAlbumMediaService _albumMediaService;

    public AlbumMediaQueryHandler(IAlbumMediaService albumMediaService)
    {
        _albumMediaService = albumMediaService;
    }

    public async Task<BusinessResult> Handle(AlbumMediaGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _albumMediaService.GetListByQueryAsync<AlbumMediaResult>(request);
    }

    public async Task<BusinessResult> Handle(AlbumMediaGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _albumMediaService.GetById<AlbumMediaResult>(request);
    }
}