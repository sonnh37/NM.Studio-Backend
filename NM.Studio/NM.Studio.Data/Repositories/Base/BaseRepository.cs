using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories.Bases;
using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Entities.Bases;
using NM.Studio.Domain.Enums;
using NM.Studio.Domain.Utilities;
using NM.Studio.Domain.Utilities.Filters;

namespace NM.Studio.Data.Repositories.Base;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbContext _dbContext;
    protected readonly IMapper Mapper;

    public BaseRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public BaseRepository(DbContext dbContext, IMapper mapper) : this(dbContext)
    {
        Mapper = mapper;
    }

    public DbSet<TEntity> DbSet
    {
        get
        {
            var dbSet = GetDbSet<TEntity>();
            return dbSet;
        }
    }

    public virtual void CheckCancellationToken(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
            throw new OperationCanceledException("Request was cancelled");
    }


    public DbSet<TEntity> Context()
    {
        return DbSet;
    }

    public static IQueryable<TEntity> Sort(IQueryable<TEntity> queryable, GetQueryableQuery query)
    {
        var sortField = query.Sorting.SortField;
        var sortDirection = query.Sorting.SortDirection;

        queryable = sortDirection == SortDirection.Ascending
            ? queryable.OrderBy(e => EF.Property<object>(e, sortField))
            : queryable.OrderByDescending(e => EF.Property<object>(e, sortField));

        return queryable;
    }

    public static IQueryable<TEntity> Include(IQueryable<TEntity> queryable, string[]? includeProperties)
    {
        if (includeProperties != null)
            foreach (var property in includeProperties)
                queryable = queryable.Include(property);

        return queryable;
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

    public void Delete(TEntity entity)
    {
        entity.IsDeleted = true;
        DbSet.Update(entity);
    }

    public void DeletePermanently(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public void DeleteRangePermanently(IEnumerable<TEntity> entities)
    {
        DbSet.RemoveRange(entities);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        var baseEntities = entities.ToList();
        var enumerable = baseEntities.Where(e => e.IsDeleted == false ? e.IsDeleted = true : e.IsDeleted = false);
        DbSet.UpdateRange(baseEntities);
    }

    #endregion

    #region Queries

    // Gets all entities without any filtering or pagination
    public async Task<List<TEntity>> GetAll()
    {
        var queryable = GetQueryable();
        var result = await queryable.ToListAsync();
        return result;
    }

    public async Task<(List<TEntity>, int)> GetListByQueryAsync(GetQueryableQuery query)
    {
        var queryable = GetQueryable();
        queryable = FilterHelper.Apply(queryable, query);
        queryable = Include(queryable, query.IncludeProperties);
        queryable = Sort(queryable, query);

        var totalCount  = queryable.Count();
        queryable = query.Pagination.IsPagingEnabled ? GetQueryablePagination(queryable, query) : queryable;

        return (await queryable.ToListAsync(), totalCount );
    }

    public async Task<int> GetTotalCount(DateTime? fromDate, DateTime? toDate)
    {
        var queryable = GetQueryable();

        if (fromDate.HasValue)
            queryable = queryable.Where(entity => entity.CreatedDate >= fromDate.Value);

        if (toDate.HasValue)
            queryable = queryable.Where(entity => entity.CreatedDate <= toDate.Value);

        return await queryable.CountAsync();
    }

    // end

    public virtual async Task<TEntity?> GetById(Guid id, string[]? includeProperties = null)
    {
        var queryable = GetQueryable(x => x.Id == id);
        queryable = includeProperties != null ? Include(queryable, includeProperties) : queryable;
        var entity = await queryable.SingleOrDefaultAsync();

        return entity;
    }

    public virtual async Task<TEntity?> GetByOptions(Expression<Func<TEntity, bool>> predicate)
    {
        var queryable = GetQueryable(predicate);
        queryable = IncludeHelper.Apply(queryable);
        var entity = await queryable.FirstOrDefaultAsync();

        return entity;
    }

    public IQueryable<TEntity> GetQueryable(CancellationToken cancellationToken = default)
    {
        CheckCancellationToken(cancellationToken);
        var queryable = GetQueryable<TEntity>();
        return queryable;
    }

    public IQueryable<T> GetQueryable<T>()
        where T : BaseEntity
    {
        IQueryable<T> queryable = GetDbSet<T>(); // like DbSet in this
        return queryable;
    }

    public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate)
    {
        var queryable = GetQueryable<TEntity>();
        queryable = queryable.Where(predicate);
        return queryable;
    }

    private DbSet<T> GetDbSet<T>() where T : BaseEntity
    {
        var dbSet = _dbContext.Set<T>();
        return dbSet;
    }

    private IQueryable<TEntity> GetQueryablePagination(IQueryable<TEntity> queryable, GetQueryableQuery query)
    {
        queryable = queryable
            .Skip((query.Pagination.PageNumber - 1) * query.Pagination.PageSize)
            .Take(query.Pagination.PageSize);
        return queryable;
    }

    public async Task<long> GetTotalCount()
    {
        var result = await GetQueryable().LongCountAsync();
        return result;
    }

    #endregion
}