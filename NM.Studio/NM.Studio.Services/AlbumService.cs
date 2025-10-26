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
    private readonly IAlbumImageRepository _albumImageRepository;
    private readonly IMediaBaseRepository _mediaBaseRepository;

    public AlbumService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _albumRepository = _unitOfWork.AlbumRepository;
        _albumImageRepository = _unitOfWork.AlbumImageRepository;
        _mediaBaseRepository = _unitOfWork.MediaBaseRepository;
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
        queryable = queryable.Sort(query.Sorting);
        queryable = queryable.Include(m => m.AlbumImages)
            .ThenInclude(n => n.Image);
        queryable = queryable.Include(query.IncludeProperties);


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
    
    public async Task<BusinessResult> SetCoverAlbum(AlbumSetCoverUpdateCommand updateCommand)
    {
        var albumImages = await _albumImageRepository.GetQueryable(m => m.AlbumId == updateCommand.AlbumId).ToListAsync();

        if (albumImages.Count <= 0) throw new DomainException("Not found images");
        var entity = albumImages.FirstOrDefault(m => m.ImageId == updateCommand.ImageId);
        
        if (entity == null)
            throw new NotFoundException(Const.NOT_FOUND_MSG);

        if (entity.IsCover) throw new DomainException("This image is already cover");
        
        foreach (var albumImage in albumImages)
        {
            albumImage.IsCover = (albumImage.ImageId == updateCommand.ImageId);
        }

        _albumImageRepository.UpdateRange(albumImages);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<AlbumImage>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> CreateWithImages(AlbumWithImagesCreateCommand createCommand)
    {
        if (createCommand.ImageIds == null || createCommand.ImageIds.Count <= 0)
            throw new DomainException("Not found images");

        // Input: A, B, C, D
        var album = await _albumRepository.GetQueryable(m => m.Id == createCommand.AlbumId)
            .SingleOrDefaultAsync();
        if (album == null) throw new DomainException("Not found album");

        // DB: A, B, E
        // 1. Lấy các AlbumImage đã tồn tại trong album
        var albumImageExisted = await _albumImageRepository
            .GetQueryable(n => n.ImageId != null && n.AlbumId == album.Id)
            .ToListAsync();

        // 2. Lấy các ImageId đã tồn tại
        var oldImageIds = albumImageExisted
            .Where(w => w.ImageId != null)
            .Select(m => m.ImageId!.Value)
            .ToList();

        // 3. Lấy các ImageId cần XÓA (có trong DB nhưng không có trong input)
        var imageIdsToRemove = oldImageIds
            .Except(createCommand.ImageIds)
            .ToList();

        // 4. Lấy các ImageId cần THÊM (có trong input nhưng không có trong DB)
        var newImageIds = createCommand.ImageIds
            .Except(oldImageIds)
            .ToList();

        // 5. Kiểm tra các ImageId mới có tồn tại trong MediaBase không
        var validNewImages = await _mediaBaseRepository
            .GetQueryable(m => newImageIds.Contains(m.Id))
            .Select(s => s.Id)
            .ToListAsync();

        // 6. Tạo AlbumImage mới để thêm
        var albumImagesToAdd = validNewImages.Select(imageId => new AlbumImage
        {
            AlbumId = album.Id,
            ImageId = imageId,
            IsCover = false,
        }).ToList();

        // 7. Lấy các AlbumImage cần xóa
        var albumImagesToRemove = albumImageExisted
            .Where(ai => imageIdsToRemove.Contains(ai.ImageId!.Value))
            .ToList();

        _albumImageRepository.AddRange(albumImagesToAdd);
        _albumImageRepository.DeleteRange(albumImagesToRemove, true);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception("Save changes failed");

        return new BusinessResult();
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