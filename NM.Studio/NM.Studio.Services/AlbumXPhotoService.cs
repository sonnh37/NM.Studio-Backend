﻿using AutoMapper;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.AlbumXPhotos;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class AlbumXPhotoService : BaseService<AlbumXPhoto>, IAlbumXPhotoService
{
    private readonly IAlbumXPhotoRepository _albumXPhotoRepository;

    public AlbumXPhotoService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _albumXPhotoRepository = _unitOfWork.AlbumXPhotoRepository;
    }

    public async Task<BusinessResult> DeleteById(AlbumXPhotoDeleteCommand command)
    {
        if (command.AlbumId == Guid.Empty || command.PhotoId == Guid.Empty)
            return ResponseHelper.DeleteData(false);

        var entity = await _albumXPhotoRepository.GetById(command);
        if (entity == null) return ResponseHelper.DeleteData(false);
        _albumXPhotoRepository.Delete(entity);
        var saveChanges = await _unitOfWork.SaveChanges();

        if (!saveChanges) return ResponseHelper.DeleteData(false);

        return ResponseHelper.DeleteData(true);
    }
}