using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Products;
using NM.Studio.Domain.CQRS.Commands.ProductXSizes;
using NM.Studio.Domain.CQRS.Commands.ProductXPhotos;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class ProductXSizeService : BaseService<ProductXSize>, IProductXSizeService
{
    private readonly IProductXSizeRepository _productXSizeRepository;

    public ProductXSizeService(IMapper mapper,
        IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        : base(mapper, unitOfWork, httpContextAccessor)
    {
        _productXSizeRepository = _unitOfWork.ProductXSizeRepository;
    }

    public async Task<BusinessResult> DeleteById(ProductXSizeDeleteCommand command)
    {
        if (command.ProductId == Guid.Empty || command.SizeId == Guid.Empty)
            return ResponseHelper.Delete(false);

        var queryable =
            _productXSizeRepository.GetQueryable(x => x.ProductId == command.ProductId && x.SizeId == command.SizeId);
        var entity = await queryable.FirstOrDefaultAsync();

        if (entity == null) return ResponseHelper.Delete(false);

        _productXSizeRepository.DeletePermanently(entity);
        var saveChanges = await _unitOfWork.SaveChanges();

        return ResponseHelper.Delete(saveChanges);
    }

    public async Task<BusinessResult> Update<TResult>(ProductXSizeUpdateCommand updateCommand)
        where TResult : BaseResult
    {
        try
        {
            // update isActive
            var product = _productXSizeRepository
                .GetQueryable(m => m.ProductId == updateCommand.ProductId && m.SizeId == updateCommand.SizeId)
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