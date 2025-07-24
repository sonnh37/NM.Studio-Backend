using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Domain.CQRS.Queries.Blogs;

public class BlogGetAllQuery : GetAllQuery
{
    public string? Title { get; set; }

    public string? Slug { get; set; }

    public string? Content { get; set; }

    public bool? IsFeatured { get; set; }

    public string? Thumbnail { get; set; }
}