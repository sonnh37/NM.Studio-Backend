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

    Task<int> GetTotalCount(DateTimeOffset? fromDate, DateTimeOffset? toDate);

    Task<(List<TEntity>, int)> GetAll(GetQueryableQuery query);

    Task<TEntity?> GetById(Guid id, string[]? includeProperties = null);
    Task<TEntity?> GetByOptions(Expression<Func<TEntity, bool>> predicate);

    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);

    void UpdateRange(IEnumerable<TEntity> entities);

    void Delete(TEntity entity, bool IsPermanent = false);

    void DeleteRange(IEnumerable<TEntity> entities, bool isPermanent = false);
    void CheckCancellationToken(CancellationToken cancellationToken = default);

}