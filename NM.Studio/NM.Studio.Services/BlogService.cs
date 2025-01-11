using AutoMapper;
using Microsoft.AspNetCore.Http;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Blogs;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class BlogService : BaseService<Blog>, IBlogService
{
    private readonly IBlogRepository _blogRepository;

    public BlogService(IMapper mapper,
        IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        : base(mapper, unitOfWork, httpContextAccessor)
    {
        _blogRepository = _unitOfWork.BlogRepository;
    }
    
    public async Task<BusinessResult> Create<TResult>(BlogCreateCommand createCommand) where TResult : BaseResult
    {
        try
        {
            if (createCommand.IsFeatured)
            {
                var blogAbout = _blogRepository.GetQueryable(m => !m.IsDeleted && m.IsFeatured).SingleOrDefault();
                if(blogAbout != null) return new ResponseBuilder()
                    .WithStatus(Const.FAIL_CODE)
                    .WithMessage("About page is already created")
                    .Build();

            }
            createCommand.Slug = SlugHelper.ToSlug(createCommand.Title);
            var blog = _blogRepository.GetQueryable(m => m.Slug == createCommand.Slug).SingleOrDefault();
            if (blog != null) return new ResponseBuilder()
                .WithStatus(Const.FAIL_CODE)
                .WithMessage("Title already exists")
                .Build();
            
            var entity = await CreateOrUpdateEntity(createCommand);
            var result = _mapper.Map<TResult>(entity);
            
            return new ResponseBuilder<TResult>()
                .WithData(result)
                .WithStatus(Const.SUCCESS_CODE)
                .WithMessage(Const.SUCCESS_SAVE_MSG)
                .Build();
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred while updating {typeof(BlogCreateCommand).Name}: {ex.Message}";
            return new ResponseBuilder()
                .WithStatus(Const.FAIL_CODE)
                .WithMessage(errorMessage)
                .Build();
        }
    }


    public async Task<BusinessResult> Update<TResult>(BlogUpdateCommand updateCommand) where TResult : BaseResult
    {
        try
        {
            if (updateCommand.IsFeatured)
            {
                var blogAbout = _blogRepository.GetQueryable(m => !m.IsDeleted && m.IsFeatured
                                                                               && m.Id != updateCommand.Id
                ).SingleOrDefault();
                if(blogAbout != null) return new ResponseBuilder()
                    .WithStatus(Const.FAIL_CODE)
                    .WithMessage("About page is already created")
                    .Build();
            }
            updateCommand.Slug = SlugHelper.ToSlug(updateCommand.Title);
            var blog = _blogRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefault();            
            
            if (blog == null) throw new Exception();
            
            // check if update input slug != current slug
            if (updateCommand.Slug != blog?.Slug)
            {
                // continue check if input slug == any slug
                var blog_ = _blogRepository.GetQueryable(m => m.Slug == updateCommand.Slug).SingleOrDefault();

                if (blog_ != null) return new ResponseBuilder()
                    .WithStatus(Const.FAIL_CODE)
                    .WithMessage("Title already exists")
                    .Build();
            }
            
            var entity = await CreateOrUpdateEntity(updateCommand);
            var result = _mapper.Map<TResult>(entity);
            
            return new ResponseBuilder<TResult>()
                .WithData(result)
                .WithStatus(Const.SUCCESS_CODE)
                .WithMessage(Const.SUCCESS_SAVE_MSG)
                .Build();
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred while updating {typeof(BlogUpdateCommand).Name}: {ex.Message}";
            return new ResponseBuilder()
                .WithStatus(Const.FAIL_CODE)
                .WithMessage(errorMessage)
                .Build();
        }
    }
}