using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Domain.CQRS.Queries.Services;

public class ServiceGetAllQuery : GetAllQuery
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Src { get; set; }
    
    public string? Slug { get; set; }
    
    public bool IsNotNullSlug { get; set; }

    public decimal? Price { get; set; }

    public TimeSpan? Duration { get; set; }

    public string? Promotion { get; set; }

    public bool IsActive { get; set; }
}