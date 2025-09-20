using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Entities.Bases;
using NM.Studio.Domain.Enums;
using NM.Studio.Domain.Models.CQRS.Queries.Base;

namespace NM.Studio.Domain.Utilities;

public static class RepoHelper
{
    public static IQueryable<TEntity> Sort<TEntity>(IQueryable<TEntity> queryable, GetQueryableQuery query)
        where TEntity : BaseEntity
    {
        var sortFieldInput = query.Sorting.SortField;
        var sortDirection = query.Sorting.SortDirection;

        var actualProp = typeof(TEntity)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(p => string.Equals(p.Name, sortFieldInput, StringComparison.OrdinalIgnoreCase));

        if (actualProp == null)
            throw new ArgumentException($"Property '{sortFieldInput}' does not exist on '{typeof(TEntity).Name}'");

        var matchedFieldName = actualProp.Name;

        queryable = sortDirection == SortDirection.Ascending
            ? queryable.OrderBy(e => EF.Property<object>(e, matchedFieldName))
            : queryable.OrderByDescending(e => EF.Property<object>(e, matchedFieldName));

        return queryable;
    }

    public static IQueryable<TEntity> Include<TEntity>(IQueryable<TEntity> queryable, string[]? includeProperties)
        where TEntity : BaseEntity
    {
        var validProperties = typeof(TEntity).GetProperties().Select(p => p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (includeProperties != null)
            foreach (var property in includeProperties)
            {
                if (string.IsNullOrWhiteSpace(property)) continue;
                var match = validProperties.FirstOrDefault(p => p.Equals(property, StringComparison.OrdinalIgnoreCase));
                if (match != null)
                    queryable = queryable.Include(match);
            }

        return queryable;
    }

    public static IQueryable<TEntity> GetQueryablePagination<TEntity>(IQueryable<TEntity> queryable,
        GetQueryableQuery query) where TEntity : BaseEntity
    {
        if (query.Pagination.IsPagingEnabled)
            queryable = queryable
                .Skip((query.Pagination.PageNumber - 1) * query.Pagination.PageSize)
                .Take(query.Pagination.PageSize);

        return queryable;
    }
}