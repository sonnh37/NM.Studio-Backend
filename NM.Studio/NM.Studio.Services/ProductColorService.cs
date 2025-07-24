using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.ProductColors;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class ProductColorService : BaseService<ProductColor>, IProductColorService
{
    private readonly IProductColorRepository _productColorRepository;

    public ProductColorService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _productColorRepository = _unitOfWork.ProductColorRepository;
    }

    public async Task<BusinessResult> DeleteById(ProductColorDeleteCommand command)
    {
        if (command.ProductId == Guid.Empty || command.ColorId == Guid.Empty)
            return BusinessResult.Fail(Const.NOT_FOUND_MSG);

        var queryable =
            _productColorRepository.GetQueryable(x =>
                x.ProductId == command.ProductId && x.ColorId == command.ColorId);
        var entity = await queryable.FirstOrDefaultAsync();

        if (entity == null)
            return BusinessResult.Fail(Const.NOT_FOUND_MSG);

        _productColorRepository.DeletePermanently(entity);
        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            return BusinessResult.Fail(Const.FAIL_SAVE_MSG);

        return BusinessResult.Success();
    }

    public async Task<BusinessResult> Update<TResult>(ProductColorUpdateCommand updateCommand)
        where TResult : BaseResult
    {
        try
        {
            // update isActive
            var product = _productColorRepository
                .GetQueryable(m => m.ProductId == updateCommand.ProductId && m.ColorId == updateCommand.ColorId)
                .SingleOrDefault();

            if (product == null)
                return BusinessResult.Fail(Const.NOT_FOUND_MSG);

            updateCommand.Id = product.Id;
            var entity = await CreateOrUpdateEntity(updateCommand);
            var result = _mapper.Map<TResult>(entity);

            return BusinessResult.Success();
        }
        catch (Exception ex)
        {
            return BusinessResult.ExceptionError(ex.Message);
        }
    }
}