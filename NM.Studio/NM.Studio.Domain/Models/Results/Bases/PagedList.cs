using System.Text.Json.Serialization;
using NM.Studio.Domain.Enums;
using NM.Studio.Domain.Models.CQRS.Queries.Base;

namespace NM.Studio.Domain.Models.Results.Bases;

public interface IPagedList<T>
{
    public IEnumerable<T> Results { get; }
    public int PageCount { get; protected set; }

    public int TotalItemCount { get; protected set; }

    public int PageNumber { get; protected set; }

    public int PageSize { get; protected set; }

    public bool HasPreviousPage { get; protected set; }

    public bool HasNextPage { get; protected set; }

    public bool IsFirstPage { get; protected set; }

    public bool IsLastPage { get; protected set; }

    public int FirstItemOnPage { get; protected set; }

    public int LastItemOnPage { get; protected set; }
}

public class PagedList<T> : IPagedList<T>
{
    public PagedList()
    {
    }

    public PagedList(IEnumerable<T> results, int pageNumber, int pageSize, int? totalCount = null)
    {
        int totalItemCount = totalCount ?? results?.Count() ?? 0;
        Results = results ?? [];
        //
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException($"pageNumber = {pageNumber}. PageNumber cannot be below 1.");
        }

        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException($"pageSize = {pageSize}. PageSize cannot be less than 1.");
        }

        if (totalItemCount < 0)
        {
            throw new ArgumentOutOfRangeException(
                $"totalItemCount = {totalItemCount}. TotalItemCount cannot be less than 0.");
        }

        // set source to blank list if superset is null to prevent exceptions
        TotalItemCount = totalItemCount;
        PageSize = pageSize;
        PageNumber = pageNumber;

        PageCount = TotalItemCount > 0
            ? (int)Math.Ceiling(TotalItemCount / (double)PageSize)
            : 0;

        bool pageNumberIsGood = PageCount > 0 && PageNumber <= PageCount;

        HasPreviousPage = pageNumberIsGood && PageNumber > 1;
        HasNextPage = pageNumberIsGood && PageNumber < PageCount;
        IsFirstPage = pageNumberIsGood && PageNumber == 1;
        IsLastPage = pageNumberIsGood && PageNumber == PageCount;

        int numberOfFirstItemOnPage = (PageNumber - 1) * PageSize + 1;

        FirstItemOnPage = pageNumberIsGood ? numberOfFirstItemOnPage : 0;

        int numberOfLastItemOnPage = numberOfFirstItemOnPage + PageSize - 1;

        LastItemOnPage = pageNumberIsGood
            ? numberOfLastItemOnPage > TotalItemCount ? TotalItemCount : numberOfLastItemOnPage
            : 0;
        
        //
    }

    public IEnumerable<T> Results { get; }
    public int PageCount { get; set; }
    public int TotalItemCount { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
    public bool IsFirstPage { get; set; }
    public bool IsLastPage { get; set; }
    public int FirstItemOnPage { get; set; }
    public int LastItemOnPage { get; set; }
    public int PageNumber { get; set; }

    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int PageSize { get; set; }
}