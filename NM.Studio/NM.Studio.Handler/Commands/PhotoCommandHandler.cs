﻿using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Photos;
using NM.Studio.Domain.Models.Responses;
using MediatR;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Handler.Commands.Base;

namespace NM.Studio.Handler.Commands;

public class PhotoCommandHandler : BaseCommandHandler,
    IRequestHandler<PhotoUpdateCommand, BusinessResult>,
    IRequestHandler<PhotoDeleteCommand, BusinessResult>,
    IRequestHandler<PhotoCreateCommand, BusinessResult>
{
    private readonly IPhotoService _photoService;

    public PhotoCommandHandler(IPhotoService photoService) : base(photoService)
    {
        _photoService = photoService;
    }

    public async Task<BusinessResult> Handle(PhotoCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _photoService.CreateOrUpdate<PhotoResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(PhotoDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _baseService.DeleteById(request.Id);
        return msgView;
    }

    public async Task<BusinessResult> Handle(PhotoUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _baseService.CreateOrUpdate<PhotoResult>(request);
        return msgView;
    }
}