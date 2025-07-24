using AutoMapper;
using Microsoft.AspNetCore.Http;
using NM.Studio.Domain.Contracts.Repositories.Bases;
using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Entities.Bases;
using NM.Studio.Domain.Models.Responses;
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

    public async Task<BusinessResult> GetById<TResult>(Guid id) where TResult : BaseResult
    {
        try
        {
            var entity = await _baseRepository.GetById(id);
            var result = _mapper.Map<TResult>(entity);
            if (result == null)
                return BusinessResult.Fail(Const.NOT_FOUND_MSG);

            return BusinessResult.Success();
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error {typeof(TResult).Name}: {ex.Message}";
            return BusinessResult.ExceptionError(errorMessage);
        }
    }


    // public async Task<BusinessResult> GetByOptions<TResult>(Expression<Func<TEntity, bool>> predicate)
    //     where TResult : BaseResult
    // {
    //     try
    //     {
    //         var entity = await _baseRepository.GetByOptions(predicate);
    //         var result = _mapper.Map<TResult>(entity);
    //         if (result == null)
    //             return new ResponseBuilder<TResult>()
    //                 .WithData(result)
    //                 .WithStatus(Const.NOT_FOUND_CODE)
    //                 .WithMessage(Const.NOT_FOUND_MSG)
    //                 .Build();
    //
    //         return new ResponseBuilder<TResult>()
    //             .WithData(result)
    //             .WithStatus(Const.SUCCESS_CODE)
    //             .WithMessage(Const.SUCCESS_READ_MSG)
    //             .Build();
    //     }
    //     catch (Exception ex)
    //     {
    //         string errorMessage = $"An error {typeof(TResult).Name}: {ex.Message}";
    //         return new ResponseBuilder()
    //             .WithStatus(Const.FAIL_CODE)
    //             .WithMessage(errorMessage)
    //             .Build();
    //     }
    // }

    public async Task<BusinessResult> GetAll<TResult>() where TResult : BaseResult
    {
        try
        {
            var entities = await _baseRepository.GetAll();
            var results = _mapper.Map<List<TResult>>(entities);

            return BusinessResult.Success(results);
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error {typeof(TResult).Name}: {ex.Message}";
            return BusinessResult.ExceptionError(errorMessage);
        }
    }

    public async Task<BusinessResult> GetListByQueryAsync<TResult>(GetQueryableQuery query) where TResult : BaseResult
    {
        try
        {
            var (entities, totalCount) = await _baseRepository.GetListByQueryAsync(query);
            var results = _mapper.Map<List<TResult>>(entities);
            var tableResponse = new QueryResult(results, totalCount, query);

            return BusinessResult.Success(tableResponse);
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred in {typeof(TResult).Name}: {ex.Message}";
            return BusinessResult.ExceptionError(errorMessage);
        }
    }

    #endregion

    #region Commands

    public async Task<BusinessResult> CreateOrUpdate<TResult>(CreateOrUpdateCommand createOrUpdateCommand)
        where TResult : BaseResult
    {
        try
        {
            var entity = await CreateOrUpdateEntity(createOrUpdateCommand);
            var result = _mapper.Map<TResult>(entity);
            if (result == null)
                return BusinessResult.Fail(Const.FAIL_SAVE_MSG);

            return BusinessResult.Success(result);
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred while updating {typeof(TEntity).Name}: {ex.Message}";
            return BusinessResult.ExceptionError(errorMessage);
        }
    }

    public async Task<BusinessResult> Restore<TResult>(UpdateCommand updateCommand)
        where TResult : BaseResult
    {
        try
        {
            TEntity? entity;

            entity = await _baseRepository.GetById(updateCommand.Id);
            if (entity == null) return BusinessResult.Fail(Const.NOT_FOUND_MSG);

            entity.IsDeleted = false;

            SetBaseEntityProperties(entity, EntityOperation.Update);
            _baseRepository.Update(entity);

            if (!await _unitOfWork.SaveChanges())
                return BusinessResult.Fail();

            var result = _mapper.Map<TResult>(entity);

            return BusinessResult.Success(result);
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred while updating {typeof(TEntity).Name}: {ex.Message}";
            return BusinessResult.ExceptionError(errorMessage);
        }
    }

    public async Task<BusinessResult> DeleteById(Guid id, bool isPermanent = false)
    {
        try
        {
            var entity = await _baseRepository.GetById(id);
            if (entity == null) return BusinessResult.Fail(Const.NOT_FOUND_MSG);

            if (isPermanent)
                _baseRepository.DeletePermanently(entity);
            else
                _baseRepository.Delete(entity);

            if (!await _unitOfWork.SaveChanges())
                return BusinessResult.Fail();

            return BusinessResult.Success();
        }
        catch (Exception ex)
        {
            return BusinessResult.Fail(ex.Message);
        }
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

        entity.LastUpdatedDate = DateTime.UtcNow;

        if (operation == EntityOperation.Create)
        {
            entity.CreatedDate = DateTime.UtcNow;
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