using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.Models.Results.Bases;

public class QueryResult
{
    public QueryResult()
    {
    }

    public QueryResult(IEnumerable<object>? results = null, int? totalCount = null, GetQueryableQuery? query = null)
    {
        Results = results ?? [];
        query ??= new GetQueryableQuery
        {
            Pagination = new PaginationParameters
            {
                IsPagingEnabled = false,
            },
            Sorting = new SortingParameters
            {
                SortField = "CreatedDate",
                SortDirection = Enums.SortDirection.Descending
            },
        };

        IsPagination = query.Pagination.IsPagingEnabled;
        IncludeProperties = query.IncludeProperties;
        SortField = query.Sorting.SortField;
        SortDirection = query.Sorting.SortDirection;
        TotalCount = totalCount ?? results?.Count();
        TotalPages = (totalCount != null && IsPagination)
            ? (int)Math.Ceiling(totalCount.Value / (double)query.Pagination.PageSize)
            : 1;

        if (!IsPagination) return;
        PageNumber = query.Pagination.PageNumber;
        PageSize = query.Pagination.PageSize;
    }

    public IEnumerable<object>? Results { get; }
    public string[]? IncludeProperties { get; protected set; }

    public int? TotalPages { get; protected set; }

    public int? TotalCount { get; protected set; }

    public int? PageNumber { get; protected set; }

    public int? PageSize { get; protected set; }

    public bool IsPagination { get; protected set; }

    public string? SortField { get; protected set; }

    public SortDirection? SortDirection { get; protected set; }
}