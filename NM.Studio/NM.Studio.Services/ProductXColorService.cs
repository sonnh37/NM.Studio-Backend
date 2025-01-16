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
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _productXColorRepository = _unitOfWork.ProductXColorRepository;
    }

    public async Task<BusinessResult> DeleteById(ProductXColorDeleteCommand command)
    {
        if (command.ProductId == Guid.Empty || command.ColorId == Guid.Empty)
            return new ResponseBuilder()
                .WithStatus(Const.NOT_FOUND_CODE)
                .WithMessage(Const.NOT_FOUND_MSG)
                .Build();
        var queryable =
            _productXColorRepository.GetQueryable(x =>
                x.ProductId == command.ProductId && x.ColorId == command.ColorId);
        var entity = await queryable.FirstOrDefaultAsync();

        if (entity == null)
            return new ResponseBuilder()
                .WithStatus(Const.NOT_FOUND_CODE)
                .WithMessage(Const.NOT_FOUND_MSG)
                .Build();

        _productXColorRepository.DeletePermanently(entity);
        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            return new ResponseBuilder()
                .WithStatus(Const.FAIL_CODE)
                .WithMessage(Const.FAIL_DELETE_MSG)
                .Build();

        return new ResponseBuilder()
            .WithStatus(Const.SUCCESS_CODE)
            .WithMessage(Const.SUCCESS_DELETE_MSG)
            .Build();
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

            if (product == null)
                return new ResponseBuilder()
                    .WithStatus(Const.NOT_FOUND_CODE)
                    .WithMessage(Const.NOT_FOUND_MSG)
                    .Build();

            updateCommand.Id = product.Id;
            var entity = await CreateOrUpdateEntity(updateCommand);
            var result = _mapper.Map<TResult>(entity);

            return new ResponseBuilder()
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