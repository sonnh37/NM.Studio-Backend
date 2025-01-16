using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Products;
using NM.Studio.Domain.CQRS.Queries.Products;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;
using Exception = System.Exception;

namespace NM.Studio.Services;

public class ProductService : BaseService<Product>, IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _categoryRepository = _unitOfWork.CategoryRepository;
        _productRepository = _unitOfWork.ProductRepository;
    }

    public async Task<BusinessResult> GetRepresentativeByCategory(ProductRepresentativeByCategoryQuery query)
    {
        try
        {
            var categories = _categoryRepository.GetQueryable(c => !c.IsDeleted)
                .OrderByDescending(m => m.LastUpdatedDate)
                .Include(c => c.SubCategories)
                .ThenInclude(sc => sc.Products)
                .ThenInclude(p => p.ProductXPhotos)
                .ThenInclude(px => px.Photo);

            var groupedCategories = await categories
                .Select(c => new ProductRepresentativeByCategoryResult
                {
                    Category = new CategoryResult
                    {
                        Id = c.Id,
                        Name = c.Name
                    },
                    Product = c.SubCategories
                        .SelectMany(sc => sc.Products)
                        .OrderBy(p => p.LastUpdatedDate)
                        .Take(1)
                        .Select(p => new ProductRepresentativeResult
                        {
                            Id = p.Id,
                            Sku = p.Sku,
                            Slug = p.Slug,
                            Src = p.ProductXPhotos
                                .Where(px => px.Photo != null)
                                .OrderBy(px => px.LastUpdatedDate)
                                .Select(px => px.Photo!.Src)
                                .FirstOrDefault()
                        })
                        .SingleOrDefault()
                })
                .ToListAsync();

            return new ResponseBuilder<ProductRepresentativeByCategoryResult>()
                .WithData(groupedCategories)
                .WithStatus(Const.SUCCESS_CODE)
                .WithMessage(Const.SUCCESS_READ_MSG)
                .Build();
        }
        catch (Exception ex)
        {
            return HandlerError(ex.Message);
        }
    }


    public async Task<BusinessResult> Create<TResult>(ProductCreateCommand createCommand) where TResult : BaseResult
    {
        try
        {
            createCommand.Slug = SlugHelper.ToSlug(createCommand.Name);
            var product = _productRepository.GetQueryable(m => m.Slug == createCommand.Slug).SingleOrDefault();
            if (product != null)
                return new ResponseBuilder()
                    .WithStatus(Const.FAIL_CODE)
                    .WithMessage("The product with name already exists.")
                    .Build();

            var entity = await CreateOrUpdateEntity(createCommand);
            var result = _mapper.Map<TResult>(entity);
            if (result == null)
                return new ResponseBuilder()
                    .WithStatus(Const.FAIL_CODE)
                    .WithMessage(Const.FAIL_SAVE_MSG)
                    .Build();

            return new ResponseBuilder<TResult>()
                .WithData(result)
                .WithStatus(Const.SUCCESS_CODE)
                .WithMessage(Const.SUCCESS_SAVE_MSG)
                .Build();
        }
        catch (Exception ex)
        {
            return HandlerError(ex.Message);
        }
    }

    public async Task<BusinessResult> Update<TResult>(ProductUpdateCommand updateCommand) where TResult : BaseResult
    {
        try
        {
            updateCommand.Slug = SlugHelper.ToSlug(updateCommand.Name);
            var product = _productRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefault();

            if (product == null) return new ResponseBuilder()
                .WithStatus(Const.NOT_FOUND_CODE)
                .WithMessage(Const.NOT_FOUND_MSG)
                .Build();

            // check if update input slug != current slug
            if (updateCommand.Slug != product?.Slug)
            {
                // continue check if input slug == any slug
                var product_ = _productRepository.GetQueryable(m => m.Slug == updateCommand.Slug).SingleOrDefault();

                if (product_ != null)
                    return new ResponseBuilder()
                        .WithStatus(Const.FAIL_CODE)
                        .WithMessage("The product with name already exists.")
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
            return HandlerError(ex.Message);
        }
    }
}