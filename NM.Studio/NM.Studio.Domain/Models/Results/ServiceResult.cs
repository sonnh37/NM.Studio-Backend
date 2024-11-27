using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class ServiceResult : BaseResult
{
    public string? Name { get; set; }
    
    public string? Slug { get; set; }

    public string? Description { get; set; }

    public string? Src { get; set; }

    public decimal? Price { get; set; }

    public TimeSpan? Duration { get; set; }

    public string? Promotion { get; set; }

    public bool IsActive { get; set; }
}