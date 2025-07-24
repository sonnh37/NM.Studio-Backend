using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.ProductSizes;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class ProductSizeService : BaseService<ProductSize>, IProductSizeService
{
    private readonly IProductSizeRepository _productSizeRepository;

    public ProductSizeService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _productSizeRepository = _unitOfWork.ProductSizeRepository;
    }

    public async Task<BusinessResult> DeleteById(ProductSizeDeleteCommand command)
    {
        if (command.ProductId == Guid.Empty || command.SizeId == Guid.Empty)
            return BusinessResult.Fail(Const.NOT_FOUND_MSG);


        var queryable =
            _productSizeRepository.GetQueryable(x => x.ProductId == command.ProductId && x.SizeId == command.SizeId);
        var entity = await queryable.FirstOrDefaultAsync();

        if (entity == null)
            return BusinessResult.Fail(Const.NOT_FOUND_MSG);

        _productSizeRepository.DeletePermanently(entity);
        var saveChanges = await _unitOfWork.SaveChanges();

        if (!saveChanges)
            return BusinessResult.Fail(Const.FAIL_SAVE_MSG);
        
        return BusinessResult.Success();
    }

    public async Task<BusinessResult> Update<TResult>(ProductSizeUpdateCommand updateCommand)
        where TResult : BaseResult
    {
        try
        {
            // update isActive
            var product = _productSizeRepository
                .GetQueryable(m => m.ProductId == updateCommand.ProductId && m.SizeId == updateCommand.SizeId)
                .SingleOrDefault();

            if (product == null) throw new Exception();
            updateCommand.Id = product.Id;
            var entity = await CreateOrUpdateEntity(updateCommand);

            var result = _mapper.Map<TResult>(entity);

            if (result == null) return BusinessResult.Fail(Const.NOT_FOUND_MSG);
            
            return BusinessResult.Success(result);
        }
        catch (Exception ex)
        {
            return BusinessResult.ExceptionError(ex.Message);
        }
    }
}