using System.ComponentModel.DataAnnotations;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.Models.CQRS.Queries.Base;

public abstract class BaseQuery
{
    public string? CreatedBy { get; set; }
    public string? LastUpdatedBy { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTimeOffset? FromDate { get; set; }
    public DateTimeOffset? ToDate { get; set; }
}

public class PaginationParameters
{
    private int _pageNumber = 1;
    private int _pageSize = 10;

    [Range(1, int.MaxValue)]
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    [Range(1, 100)]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 10 : value > 100 ? 100 : value;
    }

    public bool IsPagingEnabled { get; set; } = true;
}

public class SortingParameters
{
    private string _sortField = "CreatedDate";

    public string SortField
    {
        get => _sortField;
        set => _sortField = string.IsNullOrWhiteSpace(value) ? "CreatedDate" : value;
    }

    public SortDirection SortDirection { get; set; } = SortDirection.Descending;
}

public class GetQueryableQuery : BaseQuery
{
    private PaginationParameters _pagination = new();
    private SortingParameters _sorting = new();

    public PaginationParameters Pagination
    {
        get => _pagination;
        set => _pagination = value ?? new PaginationParameters();
    }

    public SortingParameters Sorting
    {
        get => _sorting;
        set => _sorting = value ?? new SortingParameters();
    }

    // Filtering
    public string[]? IncludeProperties { get; set; }

    public bool ValidateDateRange()
    {
        if (FromDate.HasValue && ToDate.HasValue) return FromDate.Value <= ToDate.Value;

        return true;
    }
}

// For GetByIdQuery, let's make it generic
public class GetByIdQuery : BaseQuery
{
    [Required] public Guid Id { get; set; }

    public string[]? IncludeProperties { get; set; }
}

// For GetAllQuery, let's make it generic too
public class GetAllQuery : GetQueryableQuery
{
}