﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Contracts.Repositories.Bases;

public interface IBaseRepository
{
}

public interface IBaseRepository<TEntity> : IBaseRepository
    where TEntity : BaseEntity
{
    DbSet<TEntity> Context();
    Task<bool> IsExistById(Guid id);

    IQueryable<TEntity> GetQueryable(CancellationToken cancellationToken = default);
    IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate);

    Task<long> GetTotalCount();

    Task<List<TEntity>> GetAll();

    Task<List<TEntity>> GetAll(GetQueryableQuery query);

    Task<(List<TEntity>, int)> GetPaged(GetQueryableQuery query);

    Task<List<TEntity>> ApplySortingAndPaging(IQueryable<TEntity> queryable, GetQueryableQuery pagedQuery);

    Task<TEntity?> GetById(Guid id);
    
    Task<TEntity?> GetByIdNoInclude(Guid id);

    Task<IList<TEntity>> GetByIds(IList<Guid> ids);

    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);
    void Delete(TEntity entity);
    void DeletePermanently(TEntity entity);
    void DeleteRange(IEnumerable<TEntity> entities);
    void CheckCancellationToken(CancellationToken cancellationToken = default);
}