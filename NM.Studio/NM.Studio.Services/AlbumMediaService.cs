using AutoMapper;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.AlbumMedias;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class AlbumMediaService : BaseService<AlbumMedia>, IAlbumMediaService
{
    private readonly IAlbumMediaRepository _albumMediaRepository;

    public AlbumMediaService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _albumMediaRepository = _unitOfWork.AlbumMediaRepository;
    }

    public async Task<BusinessResult> DeleteById(AlbumMediaDeleteCommand command)
    {
        if (command.AlbumId == Guid.Empty || command.MediaFileId == Guid.Empty)
            return BusinessResult.Fail(Const.NOT_FOUND_MSG);

        var entity = await _albumMediaRepository.GetById(command);
        if (entity == null)
            return BusinessResult.Fail(Const.NOT_FOUND_MSG);

        _albumMediaRepository.Delete(entity);
        var saveChanges = await _unitOfWork.SaveChanges();

        if (!saveChanges)
            return BusinessResult.Fail(Const.FAIL_SAVE_MSG);


        return BusinessResult.Success();
    }
}