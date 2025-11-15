using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories.Bases;
using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Data.Repositories.Base;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbContext _dbContext;

    public BaseRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private DbSet<TEntity> DbSet
    {
        get
        {
            var dbSet = GetDbSet();
            return dbSet;
        }
    }

    public virtual void CheckCancellationToken(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            throw new OperationCanceledException("Request was cancelled");
    }


    #region Commands

    public void Add(TEntity entity)
    {
        DbSet.Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        DbSet.AddRange(entities);
    }

    public void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        DbSet.UpdateRange(entities);
    }

    public void Delete(TEntity entity, bool isPermanent = false)
    {
        if (isPermanent)
        {
            DbSet.Remove(entity);
        }
        else
        {
            entity.IsDeleted = true;
            DbSet.Update(entity);
        }
    }


    public void DeleteRange(IEnumerable<TEntity> entities, bool isPermanent = false)
    {
        if (isPermanent)
        {
            DbSet.RemoveRange(entities);
        }
        else
        {
            var enumerable = entities.Where(e => !e.IsDeleted ? e.IsDeleted = true : e.IsDeleted = false);
            DbSet.UpdateRange(enumerable);
        }
    }

    #endregion

    #region Queries

    // public virtual async Task<(List<TEntity>, int)> GetAll(GetQueryableQuery query)
    // {
    //     var queryable = GetQueryable();
    //     queryable = RepoHelper.Include(queryable, query.IncludeProperties);
    //     queryable = RepoHelper.Sort(queryable, query);
    //     var total = queryable.CountAsync();
    //     queryable = query.Pagination.IsPagingEnabled ? RepoHelper.GetQueryablePagination(queryable, query) : queryable;
    //     var listData = queryable.ToListAsync();
    //     await Task.WhenAll(listData, total);
    //
    //     return (await listData, await total);
    // }

    // public virtual async Task<TEntity?> GetById(Guid id, string[]? includeProperties = null)
    // {
    //     var queryable = GetQueryable(x => x.Id == id);
    //     queryable = includeProperties != null
    //         ? RepoHelper.Include(queryable, includeProperties)
    //         : queryable;
    //     var entity = await queryable.SingleOrDefaultAsync();
    //
    //     return entity;
    // }

    public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate, bool isNoTracking = true)
    {
        var queryable = GetQueryable(isNoTracking);
        queryable = queryable.Where(predicate);
        return queryable;
    }

    public IQueryable<TEntity> GetQueryable(bool isNoTracking = true)
    {
        IQueryable<TEntity> queryable = GetDbSet();
        return isNoTracking ? queryable.AsNoTracking() : queryable;
    }


    private DbSet<TEntity> GetDbSet()
    {
        var dbSet = _dbContext.Set<TEntity>();
        return dbSet;
    }

    #endregion
}