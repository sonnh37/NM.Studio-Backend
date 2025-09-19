using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.CQRS.Commands.Products;
using NM.Studio.Domain.CQRS.Queries.Products;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;
using NM.Studio.Domain.Utilities.Filters;
using NM.Studio.Services.Bases;
using Exception = System.Exception;

namespace NM.Studio.Services;

public class ProductService : BaseService, IProductService
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
        var categories = _categoryRepository.GetQueryable(c => !c.IsDeleted);
        // .OrderByDescending(m => m.CreatedDate)
        // .Include(c => c.SubCategories);
        // .ThenInclude(sc => sc.Products)
        // .ThenInclude(p => p.ProductMedias)
        // .ThenInclude(px => px.MediaFile);

        var groupedCategories = await categories
            .Select(c => new ProductRepresentativeByCategoryResult
            {
                Category = new CategoryResult
                {
                    Id = c.Id,
                    Name = c.Name
                }
                // Product = c.SubCategories
                //     .SelectMany(sc => sc.Products)
                //     .OrderByDescending(p => p.LastUpdatedDate)
                //     .Take(1)
                //     .Select(p => new ProductRepresentativeResult
                //     {
                //         Id = p.Id,
                //         Sku = p.Sku,
                //         Slug = p.Slug,
                //         Src = p.ProductMedias
                //             .Where(px => px.MediaFile != null)
                //             .OrderBy(px => px.LastUpdatedDate)
                //             .Select(px => px.MediaFile!.Src)
                //             .FirstOrDefault()
                //     })
                //     .SingleOrDefault()
            })
            .ToListAsync();

        return new BusinessResult(groupedCategories);
    }

    public async Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand)
    {
        Product? entity = null;
        if (createOrUpdateCommand is ProductUpdateCommand updateCommand)
        {
            updateCommand.Slug = SlugHelper.ToSlug(updateCommand.Name);
            entity = _productRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefault();

            if (entity == null)
                throw new NotFoundException(Const.NOT_FOUND_MSG);

            // check if update input slug != current slug
            if (updateCommand.Slug != entity.Slug)
            {
                // continue check if input slug == any slug
                var isExistSlug = await _productRepository.GetQueryable(m => m.Slug == updateCommand.Slug).AnyAsync();

                if (isExistSlug)
                    throw new DomainException("The product with name already exists.");
            }

            _mapper.Map(updateCommand, entity);
            _productRepository.Update(entity);
        }
        else if (createOrUpdateCommand is ProductCreateCommand createCommand)
        {
            createCommand.Slug = SlugHelper.ToSlug(createCommand.Name);
            var product = _productRepository.GetQueryable(m => m.Slug == createCommand.Slug).SingleOrDefault();
            if (product != null)
                throw new DomainException("The product with name already exists.");
            entity = _mapper.Map<Product>(createCommand);
            if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
            entity.CreatedDate = DateTimeOffset.UtcNow;
            _productRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<ProductResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> GetAll(ProductGetAllQuery query)
    {
        var queryable = _productRepository.GetQueryable();

        queryable = FilterHelper.BaseEntity(queryable, query);
        queryable = RepoHelper.Include(queryable, query.IncludeProperties);
        queryable = RepoHelper.Sort(queryable, query);

        var totalCount = await queryable.CountAsync();
        var entities = await RepoHelper.GetQueryablePagination(queryable, query).ToListAsync();
        var results = _mapper.Map<List<ProductResult>>(entities);
        var getQueryableResult = new GetQueryableResult(results, totalCount, query);

        return new BusinessResult(getQueryableResult);
    }

    public async Task<BusinessResult> GetById(ProductGetByIdQuery request)
    {
        var queryable = _productRepository.GetQueryable(x => x.Id == request.Id);
        queryable = RepoHelper.Include(queryable, request.IncludeProperties);
        var entity = await queryable.SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException("Not found");
        var result = _mapper.Map<ProductResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> Delete(ProductDeleteCommand command)
    {
        var entity = await _productRepository.GetQueryable(x => x.Id == command.Id).SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);

        _productRepository.Delete(entity, command.IsPermanent);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        return new BusinessResult();
    }
}