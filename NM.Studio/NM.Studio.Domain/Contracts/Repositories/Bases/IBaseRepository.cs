using System.Linq.Expressions;
using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Contracts.Repositories.Bases;

public interface IBaseRepository
{
}

public interface IBaseRepository<TEntity> : IBaseRepository
    where TEntity : BaseEntity
{
    IQueryable<TEntity> GetQueryable(bool isNoTracking = true);
    IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate, bool isNoTracking = true);

    // Task<(List<TEntity>, int)> GetAll(GetQueryableQuery query);
    // Task<TEntity?> GetById(Guid id, string[]? includeProperties = null);
    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);

    void UpdateRange(IEnumerable<TEntity> entities);

    void Delete(TEntity entity, bool IsPermanent = false);

    void DeleteRange(IEnumerable<TEntity> entities, bool isPermanent = false);
    void CheckCancellationToken(CancellationToken cancellationToken = default);
}