﻿using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.Photos;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;
using MediatR;

namespace NM.Studio.Handler.Queries;

public class PhotoQueryHandler :
    IRequestHandler<PhotoGetAllQuery, TableResponse<PhotoResult>>,
    IRequestHandler<PhotoGetByIdQuery, ItemResponse<PhotoResult>>
{
    protected readonly IPhotoService _photoService;

    public PhotoQueryHandler(IPhotoService photoService)
    {
        _photoService = photoService;
    }

    public async Task<TableResponse<PhotoResult>> Handle(PhotoGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _photoService.GetAll<PhotoResult>(request);
    }

    public async Task<ItemResponse<PhotoResult>> Handle(PhotoGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _photoService.GetById<PhotoResult>(request.Id);
    }
}