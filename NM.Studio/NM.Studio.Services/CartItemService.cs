using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.CartItems;
using NM.Studio.Domain.Models.CQRS.Queries.CartItems;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;
using NM.Studio.Domain.Utilities.Filters;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class CartItemService : BaseService, ICartItemService
{
    private readonly ICartItemRepository _cartItemRepository;

    public CartItemService(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
        _cartItemRepository = _unitOfWork.CartItemRepository;
    }

    public async Task<BusinessResult> GetAll(CartItemGetAllQuery query)
    {
        var queryable = _cartItemRepository.GetQueryable();

        queryable = queryable.FilterBase(query);
        queryable = queryable.Include(query.IncludeProperties);
        queryable = queryable.Sort(query.Sorting);

        var pagedListCartItem = await queryable.ToPagedListAsync(query.Pagination.PageNumber, query.Pagination.PageSize);
        var pagedList = _mapper.Map<IPagedList<CartItemResult>>(pagedListCartItem);

        return new BusinessResult(pagedList);
    }


    public async Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand)
    {
        CartItem? entity = null;
        if (createOrUpdateCommand is CartItemUpdateCommand updateCommand)
        {
            entity = await _cartItemRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefaultAsync();

            if (entity == null)
                throw new NotFoundException(Const.NOT_FOUND_MSG);

            _mapper.Map(updateCommand, entity);
            _cartItemRepository.Update(entity);
        }
        else if (createOrUpdateCommand is CartItemCreateCommand createCommand)
        {
            entity = _mapper.Map<CartItem>(createCommand);
            if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
            entity.CreatedDate = DateTimeOffset.UtcNow;
            _cartItemRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<CartItemResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> GetById(CartItemGetByIdQuery request)
    {
        var queryable = _cartItemRepository.GetQueryable(x => x.Id == request.Id);
        queryable = RepoHelper.Include(queryable, request.IncludeProperties);
        var entity = await queryable.SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException("Not found");
        var result = _mapper.Map<CartItemResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> Delete(CartItemDeleteCommand command)
    {
        var entity = await _cartItemRepository.GetQueryable(x => x.Id == command.Id).SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);

        _cartItemRepository.Delete(entity, command.IsPermanent);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        return new BusinessResult();
    }
}