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
            return new ResponseBuilder()
                .WithStatus(Const.NOT_FOUND_CODE)
                .WithMessage(Const.NOT_FOUND_MSG)
                .Build();

        var queryable =
            _productXSizeRepository.GetQueryable(x => x.ProductId == command.ProductId && x.SizeId == command.SizeId);
        var entity = await queryable.FirstOrDefaultAsync();

        if (entity == null)  return new ResponseBuilder()
            .WithStatus(Const.NOT_FOUND_CODE)
            .WithMessage(Const.NOT_FOUND_MSG)
            .Build();

        _productXSizeRepository.DeletePermanently(entity);
        var saveChanges = await _unitOfWork.SaveChanges();

        if (!saveChanges) return new ResponseBuilder()
            .WithStatus(Const.FAIL_CODE)
            .WithMessage(Const.FAIL_DELETE_MSG)
            .Build();

        return new ResponseBuilder()
            .WithStatus(Const.SUCCESS_CODE)
            .WithMessage(Const.SUCCESS_DELETE_MSG)
            .Build();
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
            
            if (result == null) return HandlerNotFound();

            
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