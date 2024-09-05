﻿using CMS.Studio.Domain.Contracts.Services;
using CMS.Studio.Domain.CQRS.Queries.Albums;
using CMS.Studio.Domain.Models.Responses;
using CMS.Studio.Domain.Models.Results;
using MediatR;

namespace CMS.Studio.Handler.Queries;

public class AlbumQueryHandler:
    IRequestHandler<AlbumGetAllQuery, TableResponse<AlbumResult>>,
    IRequestHandler<AlbumGetByIdQuery, ItemResponse<AlbumResult>>
{
    protected readonly IAlbumService _albumService;

    public AlbumQueryHandler(IAlbumService albumService)
    {
        _albumService = albumService;
    }

    public async Task<TableResponse<AlbumResult>> Handle(AlbumGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _albumService.GetAll<AlbumResult>(request);
    }

    public async Task<ItemResponse<AlbumResult>> Handle(AlbumGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _albumService.GetById<AlbumResult>(request.Id);
    }
}