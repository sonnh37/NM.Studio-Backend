using AutoMapper;
using Microsoft.AspNetCore.Http;
using NM.Studio.Domain.Contracts.Repositories.Bases;
using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Entities.Bases;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;

namespace NM.Studio.Services.Bases;

public abstract class BaseService
{
}

public abstract class BaseService<TEntity> : BaseService, IBaseService
    where TEntity : BaseEntity
{
    private readonly IBaseRepository<TEntity> _baseRepository;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _unitOfWork;

    protected BaseService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _baseRepository = _unitOfWork.GetRepositoryByEntity<TEntity>();
        _httpContextAccessor ??= new HttpContextAccessor();
    }

    #region Queries

    public async Task<BusinessResult> GetById<TResult>(GetByIdQuery request) where TResult : BaseResult
    {
        var entity = await _baseRepository.GetById(request.Id, request.IncludeProperties);
        var result = _mapper.Map<TResult>(entity);
        if (result == null)
            return BusinessResult.Fail(Const.NOT_FOUND_MSG);

        return BusinessResult.Success(result);
    }

    public async Task<BusinessResult> GetAll<TResult>() where TResult : BaseResult
    {
        var entities = await _baseRepository.GetAll();
        var results = _mapper.Map<List<TResult>>(entities);

        return BusinessResult.Success(results);
    }

    public async Task<BusinessResult> GetAll<TResult>(GetQueryableQuery query) where TResult : BaseResult
    {
        var (entities, totalCount) = await _baseRepository.GetAll(query);
        var results = _mapper.Map<List<TResult>>(entities);
        var tableResponse = new QueryResult(results, totalCount, query);

        return BusinessResult.Success(tableResponse);
    }

    #endregion

    #region Commands

    public async Task<BusinessResult> CreateOrUpdate<TResult>(CreateOrUpdateCommand createOrUpdateCommand)
        where TResult : BaseResult
    {
        var entity = await CreateOrUpdateEntity(createOrUpdateCommand);
        var result = _mapper.Map<TResult>(entity);
        if (result == null)
            return BusinessResult.Fail(Const.FAIL_SAVE_MSG);

        return BusinessResult.Success(result);
    }

    // public async Task<BusinessResult> Restore<TResult>(UpdateCommand updateCommand)
    //     where TResult : BaseResult
    // {
    //     TEntity? entity;
    //
    //     entity = await _baseRepository.GetById(updateCommand.Id);
    //     if (entity == null) return BusinessResult.Fail(Const.NOT_FOUND_MSG);
    //
    //     entity.IsDeleted = false;
    //
    //     SetBaseEntityProperties(entity, EntityOperation.Update);
    //     _baseRepository.Update(entity);
    //
    //     if (!await _unitOfWork.SaveChanges())
    //         return BusinessResult.Fail();
    //
    //     var result = _mapper.Map<TResult>(entity);
    //
    //     return BusinessResult.Success(result);
    // }

    public async Task<BusinessResult> Delete(DeleteCommand command)
    {
        var entity = await _baseRepository.GetById(command.Id);
        if (entity == null) return BusinessResult.Fail(Const.NOT_FOUND_MSG);

        _baseRepository.Delete(entity, command.IsPermanent);

        if (!await _unitOfWork.SaveChanges())
            return BusinessResult.Fail();

        return BusinessResult.Success();
    }

    protected async Task<TEntity?> CreateOrUpdateEntity(CreateOrUpdateCommand createOrUpdateCommand)
    {
        TEntity? entity;
        if (createOrUpdateCommand is UpdateCommand updateCommand)
        {
            entity = await _baseRepository.GetById(updateCommand.Id);
            if (entity == null) return null;

            _mapper.Map(updateCommand, entity);

            SetBaseEntityProperties(entity, EntityOperation.Update);
            _baseRepository.Update(entity);
        }
        else
        {
            entity = _mapper.Map<TEntity>(createOrUpdateCommand);
            if (entity == null) return null;
            entity.Id = Guid.NewGuid();
            SetBaseEntityProperties(entity, EntityOperation.Create);
            _baseRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        return saveChanges ? entity : null;
    }


    protected void SetBaseEntityProperties<T>(T? entity, EntityOperation operation) where T : BaseEntity
    {
        if (entity == null) return;

        entity.LastUpdatedDate = DateTimeOffset.UtcNow;

        if (operation == EntityOperation.Create)
        {
            entity.CreatedDate = DateTimeOffset.UtcNow;
            entity.IsDeleted = false;
        }

        var user = GetUser();
        if (user == null) return;
        entity.LastUpdatedBy = user.Email;

        if (operation == EntityOperation.Create) entity.CreatedBy = user.Email;
    }

    #endregion

    protected User? GetUser()
    {
        if (!IsUserAuthenticated()) return null;

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            return null;

        if (httpContext.Items.TryGetValue("CurrentUser", out var userObj))
            return userObj as User;

        return null;
    }


    private bool IsUserAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;
    }
}

public enum EntityOperation
{
    Create,
    Update
}