using System.Linq.Expressions;
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
    IQueryable<TEntity> GetQueryable(CancellationToken cancellationToken = default);
    IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate);

    Task<List<TEntity>> GetAll();

    Task<int> GetTotalCount(DateTime? fromDate, DateTime? toDate);

    Task<(List<TEntity>, int)> GetListByQueryAsync(GetQueryableQuery query);

    Task<TEntity?> GetById(Guid id, string[]? includeProperties = null);
    Task<TEntity?> GetByOptions(Expression<Func<TEntity, bool>> predicate);

    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);

    void UpdateRange(IEnumerable<TEntity> entities);

    void Delete(TEntity entity);

    void DeletePermanently(TEntity entity);

    void DeleteRange(IEnumerable<TEntity> entities);

    void CheckCancellationToken(CancellationToken cancellationToken = default);

    void DeleteRangePermanently(IEnumerable<TEntity> entities);
}