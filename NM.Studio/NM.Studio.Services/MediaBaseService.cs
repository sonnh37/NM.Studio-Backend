using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.MediaBases;
using NM.Studio.Domain.Models.CQRS.Queries.MediaBases;
using NM.Studio.Domain.Models.Requests;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;
using NM.Studio.Domain.Utilities.Filters;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class MediaBaseService : BaseService, IMediaBaseService
{
    private readonly IMediaBaseRepository _mediaBaseRepository;
    private readonly IMediaUploadService _mediaUploadService;

    public MediaBaseService(IMapper mapper, IUnitOfWork unitOfWork, IMediaUploadService mediaUploadService) : base(
        mapper, unitOfWork)
    {
        _mediaUploadService = mediaUploadService;
        _mediaBaseRepository = _unitOfWork.MediaBaseRepository;
    }

    public async Task<BusinessResult> GetAll(MediaBaseGetAllQuery query)
    {
        var queryable = _mediaBaseRepository.GetQueryable();

        queryable = queryable.FilterBase(query);
        queryable = queryable.Include(query.IncludeProperties);
        queryable = queryable.Sort(query.Sorting);

        var pagedListMediaBase =
            await queryable.ToPagedListAsync(query.Pagination.PageNumber, query.Pagination.PageSize);
        var pagedList = _mapper.Map<IPagedList<MediaBaseResult>>(pagedListMediaBase);

        return new BusinessResult(pagedList);
    }

    // public Task<BusinessResult> SaveMediaWithSrc(FileUploadRequest request)
    // {
    //     throw new NotImplementedException();
    // }

    public async Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand)
    {
        MediaBase? entity = null;
        if (createOrUpdateCommand is MediaBaseUpdateCommand updateCommand)
        {
            entity = await _mediaBaseRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefaultAsync();

            if (entity == null)
                throw new NotFoundException(Const.NOT_FOUND_MSG);

            _mapper.Map(updateCommand, entity);
            _mediaBaseRepository.Update(entity);
        }
        else if (createOrUpdateCommand is MediaBaseCreateCommand createCommand)
        {
            entity = _mapper.Map<MediaBase>(createCommand);
            if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
            entity.CreatedDate = DateTimeOffset.UtcNow;
            _mediaBaseRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<MediaBaseResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> GetById(MediaBaseGetByIdQuery request)
    {
        var queryable = _mediaBaseRepository.GetQueryable(x => x.Id == request.Id);
        queryable = RepoHelper.Include(queryable, request.IncludeProperties);
        var entity = await queryable.SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException("Not found");
        var result = _mapper.Map<MediaBaseResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> Delete(MediaBaseDeleteCommand command)
    {
        var entity = await _mediaBaseRepository.GetQueryable(x => x.Id == command.Id).SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
        if (command.IsPermanent)
        {
            if (entity.MediaUrl != null) _mediaUploadService.DeleteFileAsync(entity.MediaUrl);
        }

        _mediaBaseRepository.Delete(entity, command.IsPermanent);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        return new BusinessResult();
    }
}