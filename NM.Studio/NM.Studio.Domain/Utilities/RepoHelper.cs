using System.Collections;
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
        if (includeProperties == null || !includeProperties.Any())
            return queryable;

        foreach (var propertyPath in includeProperties)
        {
            if (string.IsNullOrWhiteSpace(propertyPath)) 
                continue;

            // Tìm property path chính xác (case-insensitive)
            var correctPropertyPath = FindCorrectPropertyPath<TEntity>(propertyPath);
            if (!string.IsNullOrEmpty(correctPropertyPath))
            {
                queryable = queryable.Include(correctPropertyPath);
            }
        }

        return queryable;
    }

    private static string? FindCorrectPropertyPath<TEntity>(string propertyPath)
    {
        var parts = propertyPath.Split('.');
        var correctParts = new List<string>();
        var currentType = typeof(TEntity);

        foreach (var part in parts)
        {
            // Tìm property với case-insensitive
            var property = currentType.GetProperties()
                .FirstOrDefault(p => p.Name.Equals(part, StringComparison.OrdinalIgnoreCase));

            if (property == null)
                return null; // Property không tồn tại

            correctParts.Add(property.Name); // Sử dụng tên chính xác

            // Xử lý nested type
            currentType = property.PropertyType;

            // Xử lý collection (IEnumerable<T>)
            if (currentType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(currentType))
            {
                currentType = currentType.GetGenericArguments()[0];
            }
        }

        return string.Join(".", correctParts);
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