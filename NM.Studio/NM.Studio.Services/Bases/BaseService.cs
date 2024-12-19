using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories.Bases;
using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Entities.Bases;
using NM.Studio.Domain.Models;
using NM.Studio.Domain.Models.Responses;
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
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IHttpContextAccessor _httpContextAccessor;

    protected BaseService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor? httpContextAccessor = null)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _baseRepository = _unitOfWork.GetRepositoryByEntity<TEntity>();
        _httpContextAccessor ??= new HttpContextAccessor();
    }
    
    public User? GetUser()
    {
        try
        {
            if (_httpContextAccessor?.HttpContext == null || !_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                return null;

            // Lấy thông tin UserId từ Claims
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return null;

            // Lấy thêm thông tin User từ database nếu cần
            var userId = Guid.Parse(userIdClaim);
            var user = _unitOfWork.UserRepository.GetById(userId).Result;

            return user;
        }
        catch (Exception ex)
        {
            // Log lỗi nếu cần thiết
            return null;
        }
    }
    #region Queries

    public async Task<BusinessResult> GetById<TResult>(Guid id) where TResult : BaseResult
    {
        try
        {
            var entity = await _baseRepository.GetById(id);
            var result = _mapper.Map<TResult>(entity);
            if (result == null) return ResponseHelper.Warning<TResult>(result);
            
            return ResponseHelper.Success(result);
        }
        catch (Exception ex)
        {
            string errorMessage = $"An error {typeof(TResult).Name}: {ex.Message}";
            return ResponseHelper.Error(errorMessage);
        }
    }

    public async Task<BusinessResult> GetAll<TResult>() where TResult : BaseResult
    {
        try
        {
            var entities = await _baseRepository.GetAll();
            var results = _mapper.Map<List<TResult>>(entities);
            if (results.Count == 0) return ResponseHelper.Warning<TResult>(results);

            return ResponseHelper.Success(results);
        }
        catch (Exception ex)
        {
            string errorMessage = $"An error {typeof(TResult).Name}: {ex.Message}";
            return ResponseHelper.Error(errorMessage);
        }
    }

    public async Task<BusinessResult> GetAll<TResult>(GetQueryableQuery x) where TResult : BaseResult
    {
        try
        {
            List<TResult>? results;
            int totalItems = 0;

            if (!x.IsPagination)
            {
                var allData = await _baseRepository.GetAll(x);
                results = _mapper.Map<List<TResult>>(allData);
                if (results.Count == 0) return ResponseHelper.Warning<TResult>(results);

                return ResponseHelper.Success(results);
            }

            var tuple = await _baseRepository.GetPaged(x);
            results = _mapper.Map<List<TResult>?>(tuple.Item1);
            totalItems = tuple.Item2;

            return ResponseHelper.Success((results, totalItems), x);
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred in {typeof(TResult).Name}: {ex.Message}";
            return ResponseHelper.Error(errorMessage);
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
            if (result == null) return ResponseHelper.Warning();

            var msg = ResponseHelper.Success(result);

            return msg;
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred while updating {typeof(TEntity).Name}: {ex.Message}";
            return ResponseHelper.Error(errorMessage);
        }
    }

    public async Task<BusinessResult> DeleteById(Guid id, bool isPermanent = false)
    {
        try
        {
            if (id == Guid.Empty) return ResponseHelper.NotFound("Not found id");

            if (isPermanent)
            {
                var isDeleted = await DeleteEntityPermanently(id);
                return isDeleted ? ResponseHelper.Success() : ResponseHelper.Warning();
            }

            var entity = await DeleteEntity(id);

            return entity != null ? ResponseHelper.Success() : ResponseHelper.Warning();
        }
        catch (DbUpdateException dbEx)
        {
            if (dbEx.InnerException?.Message.Contains("FOREIGN KEY") == true)
            {
                var errorMessage = "Không thể xóa vì dữ liệu đang được tham chiếu ở bảng khác.";
                return ResponseHelper.Error(errorMessage);
            }

            throw;
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred while deleting {typeof(TEntity).Name} with ID {id}: {ex.Message}";
            return ResponseHelper.Error(errorMessage);
        }
    }

    protected async Task<TEntity?> CreateOrUpdateEntity(CreateOrUpdateCommand createOrUpdateCommand)
    {
        TEntity? entity;
        if (createOrUpdateCommand is UpdateCommand updateCommand)
        {
            entity = await _baseRepository.GetByIdNoInclude(updateCommand.Id);
            if (entity == null) return null;
            if (updateCommand.IsDeleted.HasValue)
            {
                entity.IsDeleted = updateCommand.IsDeleted.Value; // Chỉ cập nhật IsDeleted
            }
            else
            {
                _mapper.Map(updateCommand, entity); // Cập nhật các thuộc tính khác
            }

            InitializeBaseEntityForUpdate(entity);
            _baseRepository.Update(entity);
        }
        else
        {
            entity = _mapper.Map<TEntity>(createOrUpdateCommand);
            if (entity == null) return null;
            entity.Id = Guid.NewGuid();
            InitializeBaseEntityForCreate(entity);
            _baseRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        return saveChanges ? entity : default;
    }


    private void InitializeBaseEntityForCreate(TEntity? entity)
    {
        if (entity == null) return;

        var user = GetUser();

        entity.CreatedDate = DateTime.Now;
        entity.LastUpdatedDate = DateTime.Now;
        entity.IsDeleted = false;

        if (user == null) return;
        entity.CreatedBy = user.Email;
        entity.LastUpdatedBy = user.Email;
    }

    protected void InitializeBaseEntityForUpdate(TEntity? entity)
    {
        if (entity == null) return;

        var user = GetUser();

        entity.LastUpdatedDate = DateTime.Now;

        if (user == null) return;
        entity.LastUpdatedBy = user.Email;
    }

    private async Task<TEntity?> DeleteEntity(Guid id)
    {
        var entity = await _baseRepository.GetById(id);
        if (entity == null) return null;

        _baseRepository.Delete(entity);

        var saveChanges = await _unitOfWork.SaveChanges();
        return saveChanges ? entity : default;
    }

    private async Task<bool> DeleteEntityPermanently(Guid id)
    {
        var entity = await _baseRepository.GetById(id);
        if (entity == null) return false;

        _baseRepository.DeletePermanently(entity);

        var saveChanges = await _unitOfWork.SaveChanges();
        return saveChanges;
    }

    #endregion
}