using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.ProductVariants;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class ProductVariantService : BaseService, IProductVariantService
{
    private readonly IProductVariantRepository _productVariantRepository;

    public ProductVariantService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _productVariantRepository = _unitOfWork.ProductVariantRepository;
    }

    // public async Task<BusinessResult> Delete(ProductVariantDeleteCommand command)
    // {
    //     if (command.ProductId == Guid.Empty || command.VariantId == Guid.Empty)
    //         throw new NotFoundException(Const.NOT_FOUND_MSG);
    //
    //
    //     var queryable =
    //         _mediaUrlRepository.GetQueryable(x => x.ProductId == command.ProductId && x.VariantId == command.VariantId);
    //     var entity = await queryable.FirstOrDefaultAsync();
    //
    //     if (entity == null)
    //         throw new NotFoundException(Const.NOT_FOUND_MSG);
    //
    //     _mediaUrlRepository.Delete(entity, true);
    //     var saveChanges = await _unitOfWork.SaveChanges();
    //
    //     if (!saveChanges)
    //         return BusinessResult.Fail(Const.FAIL_SAVE_MSG);
    //     
    //     return BusinessResult.Success();
    // }
    //
    // public async Task<BusinessResult> Update<TResult>(ProductVariantUpdateCommand updateCommand)
    //     where TResult : BaseResult
    // {
    //     try
    //     {
    //         // update isActive
    //         var product = _mediaUrlRepository
    //             .GetQueryable(m => m.ProductId == updateCommand.ProductId && m.VariantId == updateCommand.VariantId)
    //             .SingleOrDefault();
    //
    //         if (product == null) throw new Exception();
    //         updateCommand.Id = product.Id;
    //         var entity = await CreateOrUpdateEntity(updateCommand);
    //
    //         var result = _mapper.Map<TResult>(entity);
    //
    //         if (result == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
    //         
    //         return BusinessResult.Success(result);
    //     }
    //     catch (Exception ex)
    //     {
    //         return BusinessResult.ExceptionError(ex.Message);
    //     }
    // }

    //  public async Task<BusinessResult> GetAll(ProductVariantGetAllQuery query)
    // {
    //     var queryable = _productVariantRepository.GetQueryable();
    //
    //     queryable = FilterHelper.BaseEntity(queryable, query);
    //     queryable = RepoHelper.Include(queryable, query.IncludeProperties);
    //     queryable = RepoHelper.Sort(queryable, query);
    //
    //     var totalCount = await queryable.CountAsync();
    //     var entities = await RepoHelper.GetQueryablePagination(queryable, query).ToListAsync();
    //     var results = _mapper.Map<List<ProductVariantResult>>(entities);
    //     var getQueryableResult = new GetQueryableResult(results, totalCount, query);
    //
    //     return new BusinessResult(getQueryableResult);
    // }

    public async Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand)
    {
        ProductVariant? entity = null;
        if (createOrUpdateCommand is ProductVariantUpdateCommand updateCommand)
        {
            entity = await _productVariantRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefaultAsync();

            if (entity == null)
                throw new NotFoundException(Const.NOT_FOUND_MSG);

            _mapper.Map(updateCommand, entity);
            _productVariantRepository.Update(entity);
        }
        else if (createOrUpdateCommand is ProductVariantCreateCommand createCommand)
        {
            entity = _mapper.Map<ProductVariant>(createCommand);
            if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
            entity.CreatedDate = DateTimeOffset.UtcNow;
            _productVariantRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<ProductVariantResult>(entity);

        return new BusinessResult(result);
    }

    // public async Task<BusinessResult> GetById(ProductVariantGetByIdQuery request)
    // {
    //     var queryable = _productVariantRepository.GetQueryable(x => x.Id == request.Id);
    //     queryable = RepoHelper.Include(queryable, request.IncludeProperties);
    //     var entity = await queryable.SingleOrDefaultAsync();
    //     if (entity == null) throw new NotFoundException("Not found");
    //     var result = _mapper.Map<ProductVariantResult>(entity);
    //
    //     return new BusinessResult(result);
    // }

    public async Task<BusinessResult> Delete(ProductVariantDeleteCommand command)
    {
        var entity = await _productVariantRepository.GetQueryable(x => x.Id == command.Id).SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);

        _productVariantRepository.Delete(entity, command.IsPermanent);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        return new BusinessResult();
    }
}