using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.MediaBases;
using NM.Studio.Domain.Models.CQRS.Commands.ProductMedias;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class ProductMediaService : BaseService, IProductMediaService
{
    private readonly IProductMediaRepository _productMediaRepository;
    private readonly IMediaBaseService _mediaBaseService;
    private readonly ILogger<ProductMediaService> _logger;

    public ProductMediaService(IMapper mapper,
        IMediaBaseService mediaBaseService,
        IUnitOfWork unitOfWork, ILogger<ProductMediaService> logger)
        : base(mapper, unitOfWork)
    {
        _productMediaRepository = _unitOfWork.ProductMediaRepository;
        _mediaBaseService = mediaBaseService;
        _logger = logger;
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
        ProductMedia? entity = null;
        if (createOrUpdateCommand is ProductMediaUpdateCommand updateCommand)
        {
            entity = await _productMediaRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefaultAsync();

            if (entity == null)
                throw new NotFoundException(Const.NOT_FOUND_MSG);

            _mapper.Map(updateCommand, entity);
            _productMediaRepository.Update(entity);
        }
        else if (createOrUpdateCommand is ProductMediaCreateCommand createCommand)
        {
            entity = _mapper.Map<ProductMedia>(createCommand);
            if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
            entity.CreatedDate = DateTimeOffset.UtcNow;
            _productMediaRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<ProductMediaResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> CreateList(List<ProductMediaCreateCommand> commands)
    {
        await commands.ValidateDynamicAsync();

        List<ProductMedia>? entities = null;

        entities = _mapper.Map<List<ProductMedia>>(commands);
        if (entities == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
        entities.ForEach(x => x.CreatedDate = DateTimeOffset.UtcNow);
        _productMediaRepository.AddRange(entities);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<List<ProductMediaResult>>(entities);

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


    public async Task<BusinessResult> Delete(ProductMediaDeleteCommand command)
    {
        if (command.ProductVariantId == Guid.Empty || command.MediaBaseId == Guid.Empty)
            throw new DomainException("Empty ProductVariantId or ImageId");

        var entity = await _productMediaRepository
            .GetQueryable(m => m.ProductVariantId == command.ProductVariantId &&
                               m.MediaBaseId == command.MediaBaseId).SingleOrDefaultAsync();
        if (entity == null)
            throw new NotFoundException(Const.NOT_FOUND_MSG);


        _productMediaRepository.Delete(entity, command.IsPermanent);
        var saveChanges = await _unitOfWork.SaveChanges();

        if (!saveChanges)
            throw new Exception();


        if (command.IsPermanent)
        {
            var mediaBaseDeleteCommand = new MediaBaseDeleteCommand
            {
                Id = command.MediaBaseId,
                IsPermanent = command.IsPermanent
            };
            _mediaBaseService.Delete(mediaBaseDeleteCommand);
        }

        return new BusinessResult();
    }

    public async Task<BusinessResult> DeleteList(List<ProductMediaDeleteCommand> commands)
    {
        if (commands == null || commands.Count == 0)
            throw new DomainException("Command list is empty");


        foreach (var command in commands)
        {
            if (command.ProductVariantId == Guid.Empty || command.MediaBaseId == Guid.Empty)
                throw new DomainException(
                    $"Invalid command: ProductVariantId={command.ProductVariantId}, MediaBaseId={command.MediaBaseId}");

            var entity = await _productMediaRepository
                .GetQueryable(m => m.ProductVariantId == command.ProductVariantId &&
                                   m.MediaBaseId == command.MediaBaseId)
                .SingleOrDefaultAsync();

            if (entity == null)
                throw new NotFoundException(
                    $"Not found: ProductVariantId={command.ProductVariantId}, MediaBaseId={command.MediaBaseId}");

            _productMediaRepository.Delete(entity, command.IsPermanent);
        }
        
        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception("Failed to save changes");

        // Xóa MediaBase cho các permanent delete
        foreach (var command in commands.Where(c => c.IsPermanent))
        {
            var mediaBaseDeleteCommand = new MediaBaseDeleteCommand
            {
                Id = command.MediaBaseId,
                IsPermanent = command.IsPermanent
            };
            await _mediaBaseService.Delete(mediaBaseDeleteCommand);
        }

        return new BusinessResult();
    }
}