using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NM.Studio.Domain.Entities.Bases;
using NM.Studio.Domain.Enums;
using NM.Studio.Domain.Models.CQRS.Queries.Base;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Utilities;

public static class RepoHelper
{
    public static IQueryable<TEntity> Sort<TEntity>(this IQueryable<TEntity> queryable, SortingParameters sortingParameters)
        where TEntity : BaseEntity
    {
        var sortFieldInput = sortingParameters.SortField;
        var sortDirection = sortingParameters.SortDirection;

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

    public static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> queryable, string[]? includeProperties)
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
    
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize)
    {
        if (superset == null)
            throw new ArgumentNullException(nameof (superset));
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException($"pageNumber = {pageNumber}. PageNumber cannot be below 1.");
        if (pageSize < 1)
            throw new ArgumentOutOfRangeException($"pageSize = {pageSize}. PageSize cannot be less than 1.");
        int totalCount = await superset.CountAsync<T>();

        List<T> objList;
        if (totalCount > 0)
            objList = await superset.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        else
            objList = new List<T>();
        return new PagedList<T>(objList, pageNumber, pageSize, totalCount);
    }

}