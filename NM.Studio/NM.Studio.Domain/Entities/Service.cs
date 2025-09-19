using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class Service : BaseEntity
{
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public bool IsFeatured { get; set; }
    public int? HomeSortOrder { get; set; }
    public int SortOrder { get; set; }
    public Guid? BackgroundCoverId { get; set; }
    public Guid? ThumbnailId { get; set; }
    public string? TermsAndConditions { get; set; }
    public virtual MediaBase? Thumbnail { get; set; }
    public virtual MediaBase? BackgroundCover { get; set; }
    public virtual ICollection<ServiceBooking> Bookings { get; set; } = new List<ServiceBooking>();
}