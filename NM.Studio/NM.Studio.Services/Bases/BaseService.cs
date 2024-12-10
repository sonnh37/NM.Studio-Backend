using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories.Bases;
using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.CQRS.Queries.Base;
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

    protected BaseService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _baseRepository = _unitOfWork.GetRepositoryByEntity<TEntity>();
    }

    #region Queries

 public async Task<BusinessResult> GetById<TResult>(Guid id) where TResult : BaseResult
    {
        try
        {
            var entity = await _baseRepository.GetById(id);
            var result = _mapper.Map<TResult>(entity);
            return ResponseHelper.GetOne(result);
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
            return ResponseHelper.GetAll(results);
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
                results = _mapper.Map<List<TResult>?>(allData);
                return ResponseHelper.GetAll(results);
            }

            var tuple = await _baseRepository.GetPaged(x);
            results = _mapper.Map<List<TResult>?>(tuple.Item1);
            totalItems = tuple.Item2;

            return ResponseHelper.GetAllPaginated((results, totalItems), x);
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred in {typeof(TResult).Name}: {ex.Message}";
            return ResponseHelper.Error(errorMessage);
        }
    }

    #endregion

    #region Commands

    public async Task<BusinessResult> CreateOrUpdate<TResult>(CreateOrUpdateCommand createOrUpdateCommand) where TResult : BaseResult
    {
        try
        {
            var entity = await CreateOrUpdateEntity(createOrUpdateCommand);
            var result = _mapper.Map<TResult>(entity);
            var msg = ResponseHelper.Save(result);
            
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
            if (id == Guid.Empty) return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);

            if (isPermanent)
            {
                var isDeleted = await DeleteEntityPermanently(id);
                
                return ResponseHelper.Delete(isDeleted.HasValue);
            }
            
            var entity = await DeleteEntity(id);

            return ResponseHelper.Delete(entity != null);
        }
        catch (DbUpdateException dbEx)
        {
            // Xử lý lỗi liên quan đến ràng buộc khóa ngoại
            if (dbEx.InnerException?.Message.Contains("FOREIGN KEY") == true)
            {
                var errorMessage = "Không thể xóa vì dữ liệu đang được tham chiếu ở bảng khác.";
                return ResponseHelper.Error(errorMessage);
            }
            throw; // Ném lại nếu không phải lỗi khóa ngoại
        }
        catch (Exception ex)
        {
            // Xử lý các lỗi khác
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
            if (updateCommand.IsDeleted.HasValue && updateCommand.IsDeleted.Value)
            {
                entity.IsDeleted = updateCommand.IsDeleted.Value; // Chỉ cập nhật IsDeleted
            }
            else
            {
                _mapper.Map(updateCommand, entity); // Cập nhật các thuộc tính khác
            }
            SetBaseEntityUpdate(entity);
            _baseRepository.Update(entity); 
        }
        else
        {
            entity = _mapper.Map<TEntity>(createOrUpdateCommand);
            if (entity == null) return null;
            entity.Id = Guid.NewGuid();
            SetBaseEntityCreate(entity);
            _baseRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        return saveChanges ? entity : default;
    }


    private static void SetBaseEntityCreate(TEntity? entity)
    {
        if (entity == null) return;

        var user = InformationUser.User;

        entity.CreatedDate = DateTime.Now;
        entity.LastUpdatedDate = DateTime.Now;
        entity.IsDeleted = false;

        if (user == null) return;
        entity.CreatedBy = user.Email;
        entity.LastUpdatedBy = user.Email;
    }

    private static void SetBaseEntityUpdate(TEntity? entity)
    {
        if (entity == null) return;

        var user = InformationUser.User;

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
    
    private async Task<bool?> DeleteEntityPermanently(Guid id)
    {
        var entity = await _baseRepository.GetById(id);
        if (entity == null) return null;

        _baseRepository.DeletePermanently(entity);

        var saveChanges = await _unitOfWork.SaveChanges();
        return saveChanges;
    }

    #endregion
}