using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Products;
using NM.Studio.Domain.CQRS.Queries.Products;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;
using Exception = System.Exception;

namespace NM.Studio.Services;

public class ProductService : BaseService<Product>, IProductService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;

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
                .ThenInclude(p => p.ProductMedias)
                .ThenInclude(px => px.MediaFile);

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
                        .OrderByDescending(p => p.LastUpdatedDate)
                        .Take(1)
                        .Select(p => new ProductRepresentativeResult
                        {
                            Id = p.Id,
                            Sku = p.Sku,
                            Slug = p.Slug,
                            Src = p.ProductMedias
                                .Where(px => px.MediaFile != null)
                                .OrderBy(px => px.LastUpdatedDate)
                                .Select(px => px.MediaFile!.Src)
                                .FirstOrDefault()
                        })
                        .SingleOrDefault()
                })
                .ToListAsync();

            return BusinessResult.Success(groupedCategories);
        }
        catch (Exception ex)
        {
            return BusinessResult.ExceptionError(ex.Message);
        }
    }


    public async Task<BusinessResult> Create<TResult>(ProductCreateCommand createCommand) where TResult : BaseResult
    {
        try
        {
            createCommand.Slug = SlugHelper.ToSlug(createCommand.Name);
            var product = _productRepository.GetQueryable(m => m.Slug == createCommand.Slug).SingleOrDefault();
            if (product != null)
                return BusinessResult.Fail("The product with name already exists.");

            var entity = await CreateOrUpdateEntity(createCommand);
            var result = _mapper.Map<TResult>(entity);
            if (result == null)
                return BusinessResult.Fail();

            return BusinessResult.Success(result);
        }
        catch (Exception ex)
        {
            return BusinessResult.ExceptionError(ex.Message);
        }
    }

    public async Task<BusinessResult> Update<TResult>(ProductUpdateCommand updateCommand) where TResult : BaseResult
    {
        try
        {
            updateCommand.Slug = SlugHelper.ToSlug(updateCommand.Name);
            var product = _productRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefault();

            if (product == null)
                return BusinessResult.Fail(Const.NOT_FOUND_MSG);

            // check if update input slug != current slug
            if (updateCommand.Slug != product?.Slug)
            {
                // continue check if input slug == any slug
                var product_ = _productRepository.GetQueryable(m => m.Slug == updateCommand.Slug).SingleOrDefault();

                if (product_ != null)
                    return BusinessResult.Fail("The product with name already exists.");
            }

            var entity = await CreateOrUpdateEntity(updateCommand);
            var result = _mapper.Map<TResult>(entity);
            return BusinessResult.Success(result);
        }
        catch (Exception ex)
        {
            return BusinessResult.ExceptionError(ex.Message);
        }
    }
}