using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.Albums;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Queries.Albums;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;
using NM.Studio.Domain.Utilities.Filters;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class AlbumService : BaseService, IAlbumService
{
    private readonly IAlbumRepository _albumRepository;

    public AlbumService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _albumRepository = _unitOfWork.AlbumRepository;
    }

    public async Task<BusinessResult> GetAll(AlbumGetAllQuery query)
    {
        var queryable = _albumRepository.GetQueryable();

        if (!string.IsNullOrEmpty(query.Title))
            queryable = queryable.Where(m => m.Title!.ToLower().Trim().Contains(query.Title.ToLower().Trim()));

        if (!string.IsNullOrEmpty(query.Description))
            queryable = queryable.Where(m => m.Description!.ToLower().Contains(query.Description.ToLower()));

        if (!string.IsNullOrEmpty(query.Slug))
            queryable = queryable.Where(m => m.Slug!.ToLower().Trim() == query.Slug.ToLower().Trim());

        queryable = queryable.FilterBase(query);
        queryable = queryable.Include(query.IncludeProperties);
        queryable = queryable.Sort(query.Sorting);
        queryable = queryable.Include(m => m.AlbumImages)
            .ThenInclude(n => n.Image);
        
        var pagedListAlbum = await queryable.ToPagedListAsync(query.Pagination.PageNumber, query.Pagination.PageSize);
        
        var pagedList = _mapper.Map<IPagedList<AlbumResult>>(pagedListAlbum);
        
        foreach (var albumResult in pagedList.Results)
        {
            string? coverUrl = null;
            if (albumResult.AlbumImages.Count > 0)
            {
                coverUrl = albumResult.AlbumImages
                    .Where(m => m.IsCover)
                    .Select(n => n.Image?.MediaUrl).FirstOrDefault();
            }

            albumResult.CoverUrl = coverUrl;
        }

        return new BusinessResult(pagedList);
    }

    public async Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand)
    {
        Album? entity = null;
        if (createOrUpdateCommand is AlbumUpdateCommand updateCommand)
        {
            updateCommand.Slug = SlugHelper.ToSlug(updateCommand.Title);
            entity = await _albumRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefaultAsync();

            if (entity == null)
                throw new NotFoundException(Const.NOT_FOUND_MSG);

            if (updateCommand.Slug != entity.Slug)
            {
                var slugExists = await _albumRepository
                    .GetQueryable(m => m.Slug == updateCommand.Slug && m.Id != updateCommand.Id)
                    .AnyAsync();

                if (slugExists)
                    throw new DomainException("An album with this title already exists");
            }

            _mapper.Map(updateCommand, entity);
            _albumRepository.Update(entity);
        }
        else if (createOrUpdateCommand is AlbumCreateCommand createCommand)
        {
            createCommand.Slug = SlugHelper.ToSlug(createCommand.Title);
            var isExistSlug = await _albumRepository.GetQueryable(m => m.Slug == createCommand.Slug).AnyAsync();
            if (isExistSlug) throw new DomainException("An album with this title already exists");

            entity = _mapper.Map<Album>(createCommand);
            entity.CreatedDate = DateTimeOffset.UtcNow;
            _albumRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<AlbumResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> GetById(AlbumGetByIdQuery request)
    {
        var queryable = _albumRepository.GetQueryable(x => x.Id == request.Id);
        queryable = RepoHelper.Include(queryable, request.IncludeProperties);
        var entity = await queryable.SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException("Not found");
        var result = _mapper.Map<AlbumResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> Delete(AlbumDeleteCommand command)
    {
        var entity = await _albumRepository.GetQueryable(x => x.Id == command.Id).SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);

        _albumRepository.Delete(entity, command.IsPermanent);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        return new BusinessResult();
    }
}