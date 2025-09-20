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

    public BlogService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _blogRepository = _unitOfWork.BlogRepository;
    }

    public async Task<BusinessResult> GetAll(BlogGetAllQuery query)
    {
        var queryable = _blogRepository.GetQueryable();

        queryable = FilterHelper.BaseEntity(queryable, query);
        queryable = RepoHelper.Include(queryable, query.IncludeProperties);
        queryable = RepoHelper.Sort(queryable, query);

        var totalCount = await queryable.CountAsync();
        var entities = await RepoHelper.GetQueryablePagination(queryable, query).ToListAsync();
        var results = _mapper.Map<List<BlogResult>>(entities);
        var getQueryableResult = new GetQueryableResult(results, totalCount, query);

        return new BusinessResult(getQueryableResult);
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
            if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
            entity.CreatedDate = DateTimeOffset.UtcNow;
            _blogRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<AlbumResult>(entity);

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
        var entity = await _blogRepository.GetQueryable(x => x.Id == command.Id).SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);

        _blogRepository.Delete(entity, command.IsPermanent);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        return new BusinessResult();
    }
}