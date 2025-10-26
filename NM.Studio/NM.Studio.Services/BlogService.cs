using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.Blogs;
using NM.Studio.Domain.Models.CQRS.Queries.Blogs;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;
using NM.Studio.Domain.Utilities.Filters;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class BlogService : BaseService, IBlogService
{
    private readonly IBlogRepository _blogRepository;
    private readonly IMediaUploadService _mediaUploadService;
    private readonly IMediaBaseService _mediaBaseService;

    public BlogService(IMapper mapper,
        IMediaUploadService mediaUploadService,
        IMediaBaseService mediaBaseService,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _mediaBaseService = mediaBaseService;
        _mediaUploadService = mediaUploadService;
        _blogRepository = _unitOfWork.BlogRepository;
    }

    public async Task<BusinessResult> GetAll(BlogGetAllQuery query)
    {
        var queryable = _blogRepository.GetQueryable();

        queryable = queryable.FilterBase(query);
        if (!string.IsNullOrEmpty(query.Slug))
        {
            queryable = queryable.Where(n => n.Slug == query.Slug);
        }
        queryable = queryable.Include(query.IncludeProperties);
        queryable = queryable.Sort(query.Sorting);

        var pagedListBlog = await queryable.ToPagedListAsync(query.Pagination.PageNumber, query.Pagination.PageSize);
        var pagedList = _mapper.Map<IPagedList<BlogResult>>(pagedListBlog);

        return new BusinessResult(pagedList);
    }

    public async Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand)
    {
        Blog? entity = null;
        if (createOrUpdateCommand is BlogUpdateCommand updateCommand)
        {
            if (updateCommand.IsFeatured)
            {
                var blogAbout = _blogRepository.GetQueryable(m => !m.IsDeleted && m.IsFeatured
                                                                               && m.Id != updateCommand.Id
                ).SingleOrDefault();
                if (blogAbout != null)
                    throw new DomainException("About page is already created");
            }

            updateCommand.Slug = SlugHelper.ToSlug(updateCommand.Title);
            entity = _blogRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefault();

            if (entity == null) throw new Exception();

            // check if update input slug != current slug
            if (updateCommand.Slug != entity.Slug)
            {
                // continue check if input slug == any slug
                var blog_ = _blogRepository.GetQueryable(m => m.Slug == updateCommand.Slug).SingleOrDefault();

                if (blog_ != null)
                    throw new DomainException("Title already exists");
            }
            
            _mapper.Map(updateCommand, entity);
            
            // if (!string.IsNullOrEmpty(updateCommand.srcBackgroundCover))
            // {
            //     var mediaBaseBackgroundCover = await _mediaBaseService.CreateMediaBaseFromSrc(updateCommand.srcBackgroundCover);
            //     entity.BackgroundCoverId = mediaBaseBackgroundCover?.Id;
            // }
            //
            // if (!string.IsNullOrEmpty(updateCommand.srcThumbnail))
            // {
            //     var mediaBaseThumbnail = await _mediaBaseService.CreateMediaBaseFromSrc(updateCommand.srcThumbnail);
            //     entity.ThumbnailId = mediaBaseThumbnail?.Id;
            // }

            
            _blogRepository.Update(entity);
        }
        else if (createOrUpdateCommand is BlogCreateCommand createCommand)
        {
            if (createCommand.IsFeatured)
            {
                var blogAbout = _blogRepository.GetQueryable(m => !m.IsDeleted && m.IsFeatured).SingleOrDefault();
                if (blogAbout != null)
                    throw new DomainException("About page is already created");
            }

            createCommand.Slug = SlugHelper.ToSlug(createCommand.Title);
            var blog = _blogRepository.GetQueryable(m => m.Slug == createCommand.Slug).SingleOrDefault();
            if (blog != null)
                throw new DomainException("Title already exists");

            entity = _mapper.Map<Blog>(createCommand);
            // var mediaBaseBackgroundCover = await _mediaBaseService.CreateMediaBaseFromSrc(createCommand.srcBackgroundCover);
            // var mediaBaseThumbnail = await _mediaBaseService.CreateMediaBaseFromSrc(createCommand.srcThumbnail);
            // entity.BackgroundCoverId = mediaBaseBackgroundCover?.Id;
            // entity.ThumbnailId = mediaBaseThumbnail?.Id;
            if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
            entity.CreatedDate = DateTimeOffset.UtcNow;
            _blogRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<BlogResult>(entity);

        return new BusinessResult(result);
    }


    public async Task<BusinessResult> GetById(BlogGetByIdQuery request)
    {
        var queryable = _blogRepository.GetQueryable(x => x.Id == request.Id);
        queryable = RepoHelper.Include(queryable, request.IncludeProperties);
        var entity = await queryable.SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException("Not found");
        var result = _mapper.Map<BlogResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> Delete(BlogDeleteCommand command)
    {
        var queryable = _blogRepository.GetQueryable(x => x.Id == command.Id);
        queryable = RepoHelper.Include(queryable, ["thumbnail.image.mediaUrl"]);
        var entity = await queryable.SingleOrDefaultAsync();

        if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);

        _blogRepository.Delete(entity, command.IsPermanent);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var src = entity.Thumbnail?.MediaUrl;
        if (!string.IsNullOrEmpty(src))
        {
            var res = await _mediaUploadService.DeleteFileAsync(src);
        }

        return new BusinessResult();
    }
}