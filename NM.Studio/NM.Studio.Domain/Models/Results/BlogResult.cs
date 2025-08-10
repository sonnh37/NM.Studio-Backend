using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class BlogResult : BaseResult
{
    public string? Title { get; set; }
    
    public string? Slug { get; set; }
    
    public string? Content { get; set; }
    
    public string? Summary { get; set; }
    
    public string? Thumbnail { get; set; }
    
    public string? BannerImage { get; set; }
    
    public BlogStatus Status { get; set; }
    
    public bool IsFeatured { get; set; }
    
    public int ViewCount { get; set; }
    
    public string? Tags { get; set; }
    public Guid? AuthorId { get; set; }
    public UserResult? Author { get; set; }
}