

using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Utilities.Filters;

public static class BaseFilterHelper
{
    public static IQueryable<TEntity> Base<TEntity>(IQueryable<TEntity> queryable, GetQueryableQuery query)
        where TEntity : BaseEntity
    {
        if (query.Id != null) queryable = queryable.Where(m => m.Id == query.Id);

        if (!string.IsNullOrEmpty(query.CreatedBy))
            queryable = queryable.Where(m => m.CreatedBy != null && m.CreatedBy.Contains(query.CreatedBy));

        if (!string.IsNullOrEmpty(query.LastUpdatedBy))
            queryable = queryable.Where(m => m.LastUpdatedBy != null && m.LastUpdatedBy.Contains(query.LastUpdatedBy));

        if (query.IsDeleted != null)
        {
            queryable = queryable.Where(m => query.IsDeleted.Value == m.IsDeleted);
        }

        queryable = FromDateToDate(queryable, query);

        return queryable;
    }

    private static IQueryable<TEntity> FromDateToDate<TEntity>(IQueryable<TEntity> queryable, GetQueryableQuery query)
        where TEntity : BaseEntity
    {
        if (query.FromDate.HasValue)
            queryable = queryable.Where(entity => entity.CreatedDate >= query.FromDate.Value);

        if (query.ToDate.HasValue)
            queryable = queryable.Where(entity => entity.CreatedDate <= query.ToDate.Value);

        return queryable;
    }
}