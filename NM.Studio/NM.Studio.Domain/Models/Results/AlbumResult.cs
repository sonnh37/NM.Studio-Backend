using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class AlbumResult : BaseResult
{
    public string? Title { get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? EventDate { get; set; }
    public string? BrideName { get; set; }
    public string? GroomName { get; set; }
    public string? Location { get; set; }
    public string? Photographer { get; set; }
    public int SortOrder { get; set; }
    public bool IsFeatured { get; set; }
    public int? HomeSortOrder { get; set; }
    public ICollection<AlbumImageResult> AlbumImages { get; set; } = new List<AlbumImageResult>();
}