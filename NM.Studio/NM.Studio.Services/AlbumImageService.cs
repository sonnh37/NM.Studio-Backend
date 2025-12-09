using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.AlbumImages;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.MediaBases;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class AlbumImageService : BaseService, IAlbumImageService
{
    private readonly IAlbumImageRepository _albumImageRepository;
    private readonly IMediaBaseService _mediaBaseService;

    public AlbumImageService(IMapper mapper,
        IUnitOfWork unitOfWork, IMediaBaseService mediaBaseService)
        : base(mapper, unitOfWork)
    {
        _mediaBaseService = mediaBaseService;
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
    
    // public async Task<BusinessResult> CreateWithImages(List<AlbumImageCreateCommand> createCommand)
    // {
    //     if (createCommand.ImageIds == null || createCommand.ImageIds.Count <= 0)
    //         throw new DomainException("Not found images");
    //
    //     // Input: A, B, C, D
    //     var album = await _albumRepository.GetQueryable(m => m.Id == createCommand.AlbumId)
    //         .SingleOrDefaultAsync();
    //     if (album == null) throw new DomainException("Not found album");
    //
    //     // DB: A, B, E
    //     // 1. Lấy các AlbumImage đã tồn tại trong album
    //     var albumImageExisted = await _albumImageRepository
    //         .GetQueryable(n => n.ImageId != null && n.AlbumId == album.Id)
    //         .ToListAsync();
    //
    //     // 2. Lấy các ImageId đã tồn tại
    //     var oldImageIds = albumImageExisted
    //         .Where(w => w.ImageId != null)
    //         .Select(m => m.ImageId!.Value)
    //         .ToList();
    //
    //     // 3. Lấy các ImageId cần XÓA (có trong DB nhưng không có trong input)
    //     var imageIdsToRemove = oldImageIds
    //         .Except(createCommand.ImageIds)
    //         .ToList();
    //
    //     // 4. Lấy các ImageId cần THÊM (có trong input nhưng không có trong DB)
    //     var newImageIds = createCommand.ImageIds
    //         .Except(oldImageIds)
    //         .ToList();
    //
    //     // 5. Kiểm tra các ImageId mới có tồn tại trong MediaBase không
    //     var validNewImages = await _mediaBaseRepository
    //         .GetQueryable(m => newImageIds.Contains(m.Id))
    //         .Select(s => s.Id)
    //         .ToListAsync();
    //
    //     // 6. Tạo AlbumImage mới để thêm
    //     var albumImagesToAdd = validNewImages.Select(imageId => new AlbumImage
    //     {
    //         AlbumId = album.Id,
    //         ImageId = imageId,
    //         IsCover = false,
    //     }).ToList();
    //
    //     // 7. Lấy các AlbumImage cần xóa
    //     var albumImagesToRemove = albumImageExisted
    //         .Where(ai => imageIdsToRemove.Contains(ai.ImageId!.Value))
    //         .ToList();
    //
    //     _albumImageRepository.AddRange(albumImagesToAdd);
    //     _albumImageRepository.DeleteRange(albumImagesToRemove, true);
    //
    //     var saveChanges = await _unitOfWork.SaveChanges();
    //     if (!saveChanges)
    //         throw new Exception("Save changes failed");
    //
    //     return new BusinessResult();
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
    
    public async Task<BusinessResult> CreateList(List<AlbumImageCreateCommand> commands)
    {
        await commands.ValidateDynamicAsync();

        List<AlbumImage>? entities = null;

        entities = _mapper.Map<List<AlbumImage>>(commands);
        if (entities == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
        entities.ForEach(x => x.CreatedDate = DateTimeOffset.UtcNow);
        _albumImageRepository.AddRange(entities);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<List<AlbumImageResult>>(entities);

        return new BusinessResult(result);
    }
    
    public async Task<BusinessResult> DeleteList(List<AlbumImageDeleteCommand> commands)
    {
        if (commands == null || commands.Count == 0)
            throw new DomainException("Command list is empty");


        foreach (var command in commands)
        {
            var entity = await _albumImageRepository
                .GetQueryable(m => m.AlbumId == command.AlbumId &&
                                   m.ImageId == command.ImageId)
                .SingleOrDefaultAsync();

            if (entity == null)
                throw new NotFoundException(
                    $"Not found: AlbumId={command.AlbumId}, ImageId={command.ImageId}");

            _albumImageRepository.Delete(entity, command.IsPermanent);
        }
        
        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception("Failed to save changes");

        // Xóa MediaBase cho các permanent delete
        foreach (var command in commands.Where(c => c.IsPermanent))
        {
            var mediaBaseDeleteCommand = new MediaBaseDeleteCommand
            {
                Id = command.ImageId,
                IsPermanent = command.IsPermanent
            };
            await _mediaBaseService.Delete(mediaBaseDeleteCommand);
        }

        return new BusinessResult();
    }
}