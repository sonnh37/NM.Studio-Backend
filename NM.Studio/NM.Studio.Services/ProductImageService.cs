using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.CQRS.Commands.ProductImages;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class ProductImageService : BaseService, IProductImageService
{
    private readonly IProductImageRepository _productImageRepository;

    public ProductImageService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _productImageRepository = _unitOfWork.ProductImageRepository;
    }

    // public async Task<BusinessResult> GetAll(ProductImageGetAllQuery query)
    //  {
    //      var queryable = _productImageRepository.GetQueryable();
    //
    //      queryable = FilterHelper.BaseEntity(queryable, query);
    //      queryable = RepoHelper.Include(queryable, query.IncludeProperties);
    //      queryable = RepoHelper.Sort(queryable, query);
    //
    //      var totalCount = await queryable.CountAsync();
    //      var entities = await RepoHelper.GetQueryablePagination(queryable, query).ToListAsync();
    //      var results = _mapper.Map<List<ProductImageResult>>(entities);
    //      var getQueryableResult = new GetQueryableResult(results, totalCount, query);
    //
    //      return new BusinessResult(getQueryableResult);
    //  }

    public async Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand)
    {
        ProductImage? entity = null;
        if (createOrUpdateCommand is ProductImageUpdateCommand updateCommand)
        {
            entity = await _productImageRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefaultAsync();

            if (entity == null)
                throw new NotFoundException(Const.NOT_FOUND_MSG);

            _mapper.Map(updateCommand, entity);
            _productImageRepository.Update(entity);
        }
        else if (createOrUpdateCommand is ProductImageCreateCommand createCommand)
        {
            entity = _mapper.Map<ProductImage>(createCommand);
            if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
            entity.CreatedDate = DateTimeOffset.UtcNow;
            _productImageRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<ProductImageResult>(entity);

        return new BusinessResult(result);
    }

    // public async Task<BusinessResult> GetById(ProductImageGetByIdQuery request)
    // {
    //     var queryable = _productImageRepository.GetQueryable(x => x.Id == request.Id);
    //     queryable = RepoHelper.Include(queryable, request.IncludeProperties);
    //     var entity = await queryable.SingleOrDefaultAsync();
    //     if (entity == null) throw new NotFoundException("Not found");
    //     var result = _mapper.Map<ProductImageResult>(entity);
    //
    //     return new BusinessResult(result);
    // }


    public async Task<BusinessResult> Delete(ProductImageDeleteCommand command)
    {
        if (command.ProductVariantId == Guid.Empty || command.ImageId == Guid.Empty)
            throw new DomainException("Empty ProductVariantId or ImageId");

        var entity = await _productImageRepository
            .GetQueryable(m => m.ProductVariantId == command.ProductVariantId &&
                               m.ImageId == command.ImageId).SingleOrDefaultAsync();
        if (entity == null)
            throw new NotFoundException(Const.NOT_FOUND_MSG);

        _productImageRepository.Delete(entity);
        var saveChanges = await _unitOfWork.SaveChanges();

        if (!saveChanges)
            throw new Exception();

        return new BusinessResult();
    }
}