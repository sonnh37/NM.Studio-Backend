using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class ServiceResult : BaseResult
{
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public bool IsFeatured { get; set; }
    public int? HomeSortOrder { get; set; }
    
    public Guid? BackgroundCoverId { get; set; }
    public Guid? ThumbnailId { get; set; }
    public string? TermsAndConditions { get; set; }
    public MediaBaseResult? Thumbnail { get; set; }
    public MediaBaseResult? BackgroundCover { get; set; }
    public ICollection<ServiceBookingResult> Bookings { get; set; } = new List<ServiceBookingResult>();
}