using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.CQRS.Queries.Blogs;

public class BlogGetByIdQuery : GetByIdQuery
{
    public string? Title { get; set; }

    public string? Slug { get; set; }

    public string? Content { get; set; }

    public string? Summary { get; set; }

    public string? Thumbnail { get; set; }

    public string? BannerImage { get; set; }

    public BlogStatus? Status { get; set; }

    public bool? IsFeatured { get; set; }

    public int? ViewCount { get; set; }

    public string? Tags { get; set; }
    
    public Guid? AuthorId { get; set; }
}