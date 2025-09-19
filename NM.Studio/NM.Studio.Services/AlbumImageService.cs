using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.AlbumImages;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class AlbumImageService : BaseService, IAlbumImageService
{
    private readonly IAlbumImageRepository _albumImageRepository;

    public AlbumImageService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _albumImageRepository = _unitOfWork.AlbumImageRepository;
    }

    // public async Task<BusinessResult> GetAll(AlbumImageGetAllQuery query)
    // {
    //     var queryable = _albumImageRepository.GetQueryable();
    //
    //     if (!string.IsNullOrEmpty(query.Title))
    //         queryable = queryable.Where(m => m.Title!.ToLower().Trim().Contains(query.Title.ToLower().Trim()));
    //
    //     if (!string.IsNullOrEmpty(query.Description))
    //         queryable = queryable.Where(m => m.Description!.ToLower().Contains(query.Description.ToLower()));
    //
    //     if (!string.IsNullOrEmpty(query.Slug))
    //         queryable = queryable.Where(m => m.Slug!.ToLower().Trim() == query.Slug.ToLower().Trim());
    //
    //     queryable = FilterHelper.BaseEntity(queryable, query);
    //     queryable = RepoHelper.Include(queryable, query.IncludeProperties);
    //     queryable = RepoHelper.Sort(queryable, query);
    //
    //     var totalCount = await queryable.CountAsync();
    //     var entities = await RepoHelper.GetQueryablePagination(queryable, query).ToListAsync();
    //     var results = _mapper.Map<List<AlbumResult>>(entities);
    //     var getQueryableResult = new GetQueryableResult(results, totalCount, query);
    //
    //     return new BusinessResult(getQueryableResult);
    // }

    // public async Task<BusinessResult> GetById(AlbumImageGetByIdQuery request)
    // {
    //     var queryable = _albumImageRepository.GetQueryable(x => x.Id == request.Id);
    //     queryable = RepoHelper.Include(queryable, request.IncludeProperties);
    //     var entity = await queryable.SingleOrDefaultAsync();
    //     if (entity == null) throw new NotFoundException("Not found");
    //     var result = _mapper.Map<AlbumImageResult>(entity);
    //
    //     return new BusinessResult(result);
    // }

    public async Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand)
    {
        AlbumImage? entity = null;
        if (createOrUpdateCommand is AlbumImageUpdateCommand updateCommand)
        {
            entity = await _albumImageRepository
                .GetQueryable(x => x.AlbumId == updateCommand.AlbumId && x.ImageId == updateCommand.ImageId)
                .SingleOrDefaultAsync();
            if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
            _mapper.Map(updateCommand, entity);
            _albumImageRepository.Update(entity);
        }
        else if (createOrUpdateCommand is AlbumImageCreateCommand createCommand)
        {
            entity = _mapper.Map<AlbumImage>(createCommand);
            if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
            entity.CreatedDate = DateTimeOffset.UtcNow;
            _albumImageRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<AlbumImageResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> Delete(AlbumImageDeleteCommand command)
    {
        if (command.AlbumId == Guid.Empty || command.ImageId == Guid.Empty)
            throw new DomainException("AlbumId and ImageId is required.");

        var entity = await _albumImageRepository
            .GetQueryable(x => x.AlbumId == command.AlbumId && x.ImageId == command.ImageId)
            .SingleOrDefaultAsync();
        if (entity == null)
            throw new NotFoundException(Const.NOT_FOUND_MSG);

        _albumImageRepository.Delete(entity, command.IsPermanent);
        var saveChanges = await _unitOfWork.SaveChanges();

        if (!saveChanges)
            throw new Exception();

        return new BusinessResult();
    }
}