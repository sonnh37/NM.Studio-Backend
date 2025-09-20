using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.Carts;
using NM.Studio.Domain.Models.CQRS.Queries.Carts;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;
using NM.Studio.Domain.Utilities.Filters;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class CartService : BaseService, ICartService
{
    private readonly ICartRepository _cartRepository;

    public CartService(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
        _cartRepository = _unitOfWork.CartRepository;
    }

    public async Task<BusinessResult> GetAll(CartGetAllQuery query)
    {
        var queryable = _cartRepository.GetQueryable();

        queryable = FilterHelper.BaseEntity(queryable, query);
        queryable = RepoHelper.Include(queryable, query.IncludeProperties);
        queryable = RepoHelper.Sort(queryable, query);

        var totalCount = await queryable.CountAsync();
        var entities = await RepoHelper.GetQueryablePagination(queryable, query).ToListAsync();
        var results = _mapper.Map<List<CartResult>>(entities);
        var getQueryableResult = new GetQueryableResult(results, totalCount, query);

        return new BusinessResult(getQueryableResult);
    }

    public async Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand)
    {
        Cart? entity = null;
        if (createOrUpdateCommand is CartUpdateCommand updateCommand)
        {
            entity = await _cartRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefaultAsync();

            if (entity == null)
                throw new NotFoundException(Const.NOT_FOUND_MSG);

            _mapper.Map(updateCommand, entity);
            _cartRepository.Update(entity);
        }
        else if (createOrUpdateCommand is CartCreateCommand createCommand)
        {
            entity = _mapper.Map<Cart>(createCommand);
            if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
            entity.CreatedDate = DateTimeOffset.UtcNow;
            _cartRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<CartResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> GetById(CartGetByIdQuery request)
    {
        var queryable = _cartRepository.GetQueryable(x => x.Id == request.Id);
        queryable = RepoHelper.Include(queryable, request.IncludeProperties);
        var entity = await queryable.SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException("Not found");
        var result = _mapper.Map<CartResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> Delete(CartDeleteCommand command)
    {
        var entity = await _cartRepository.GetQueryable(x => x.Id == command.Id).SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);

        _cartRepository.Delete(entity, command.IsPermanent);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        return new BusinessResult();
    }
}