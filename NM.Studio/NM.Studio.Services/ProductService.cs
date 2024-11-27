using AutoMapper;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Products;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class ProductService : BaseService<Product>, IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _productRepository = _unitOfWork.ProductRepository;
    }
    
    public async Task<BusinessResult> Create<TResult>(ProductCreateCommand createCommand) where TResult : BaseResult
    {
        try
        {
            createCommand.Slug = SlugHelper.ToSlug(createCommand.Name);
            var product = _productRepository.GetQueryable(m => m.Slug == createCommand.Slug).SingleOrDefault();
            if (product != null) return ResponseHelper.Error("Name is already in use"); 
            
            var entity = await CreateOrUpdateEntity(createCommand);
            var result = _mapper.Map<TResult>(entity);
            var msg = ResponseHelper.SaveData(result);
            
            return msg;
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred while updating {typeof(ProductCreateCommand).Name}: {ex.Message}";
            return ResponseHelper.Error(errorMessage);
        }
    }
    
    public async Task<BusinessResult> Update<TResult>(ProductUpdateCommand updateCommand) where TResult : BaseResult
    {
        try
        {
            updateCommand.Slug = SlugHelper.ToSlug(updateCommand.Name);
            var product = _productRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefault();            
            
            if (product == null) throw new Exception();
            
            // check if update input slug != current slug
            if (updateCommand.Slug != product?.Slug)
            {
                // continue check if input slug == any slug
                var product_ = _productRepository.GetQueryable(m => m.Slug == updateCommand.Slug).SingleOrDefault();

                if (product_ != null) return ResponseHelper.Error("Name is already in use"); 
            }
            
            var entity = await CreateOrUpdateEntity(updateCommand);
            var result = _mapper.Map<TResult>(entity);
            var msg = ResponseHelper.SaveData(result);
            
            return msg;
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred while updating {typeof(ProductUpdateCommand).Name}: {ex.Message}";
            return ResponseHelper.Error(errorMessage);
        }
    }
}