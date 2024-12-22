using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Products;
using NM.Studio.Domain.CQRS.Commands.ProductXColors;
using NM.Studio.Domain.CQRS.Commands.ProductXPhotos;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class ProductXColorService : BaseService<ProductXColor>, IProductXColorService
{
    private readonly IProductXColorRepository _productXColorRepository;

    public ProductXColorService(IMapper mapper,
        IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        : base(mapper, unitOfWork, httpContextAccessor)
    {
        _productXColorRepository = _unitOfWork.ProductXColorRepository;
    }

    public async Task<BusinessResult> DeleteById(ProductXColorDeleteCommand command)
    {
        if (command.ProductId == Guid.Empty || command.ColorId == Guid.Empty)
            return ResponseHelper.Delete(false);

        var queryable = _productXColorRepository.GetQueryable(x => x.ProductId == command.ProductId && x.ColorId == command.ColorId);
        var entity = await queryable.FirstOrDefaultAsync();      
        
        if (entity == null) return ResponseHelper.Delete(false);
        
        _productXColorRepository.DeletePermanently(entity);
        var saveChanges = await _unitOfWork.SaveChanges();

        return ResponseHelper.Delete(saveChanges);
    }
    
    public async Task<BusinessResult> Update<TResult>(ProductXColorUpdateCommand updateCommand)
        where TResult : BaseResult
    {
        try
        {
            // update isActive
            var product = _productXColorRepository
                .GetQueryable(m => m.ProductId == updateCommand.ProductId && m.ColorId == updateCommand.ColorId)
                .SingleOrDefault();

            if (product == null) throw new Exception();
            updateCommand.Id = product.Id;
            var entity = await CreateOrUpdateEntity(updateCommand);
            var result = _mapper.Map<TResult>(entity);
            var msg = ResponseHelper.Success(result);

            return msg;
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred while updating {typeof(ProductUpdateCommand).Name}: {ex.Message}";
            return ResponseHelper.Error(errorMessage);
        }
    }
}