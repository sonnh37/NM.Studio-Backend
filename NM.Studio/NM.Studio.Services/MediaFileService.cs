using AutoMapper;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Entities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class MediaFileService : BaseService<MediaFile>, IMediaFileService
{
    private readonly IMediaFileRepository _mediaFileRepository;

    public MediaFileService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _mediaFileRepository = _unitOfWork.MediaFileRepository;
    }
}