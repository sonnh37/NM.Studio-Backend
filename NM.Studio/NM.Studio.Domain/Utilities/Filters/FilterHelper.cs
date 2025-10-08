using NM.Studio.Domain.Entities.Bases;
using NM.Studio.Domain.Models.CQRS.Queries.Base;

namespace NM.Studio.Domain.Utilities.Filters;

public static class FilterHelper
{
    public static IQueryable<TEntity> FilterBase<TEntity>(this IQueryable<TEntity> queryable, BaseQuery query)
        where TEntity : BaseEntity
    {
        if (query.IsDeleted != null) queryable = queryable.Where(m => query.IsDeleted.Value == m.IsDeleted);

        queryable = FromDateToDate(queryable, query);

        return queryable;
    }

    private static IQueryable<TEntity> FromDateToDate<TEntity>(IQueryable<TEntity> queryable, BaseQuery query)
        where TEntity : BaseEntity
    {
        if (query.FromDate.HasValue)
            queryable = queryable.Where(entity => entity.CreatedDate >= query.FromDate.Value);

        if (query.ToDate.HasValue)
            queryable = queryable.Where(entity => entity.CreatedDate <= query.ToDate.Value);

        return queryable;
    }
}