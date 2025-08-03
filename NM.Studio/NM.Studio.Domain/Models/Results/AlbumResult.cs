using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class AlbumResult : BaseResult
{
    public string? Title { get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public string? Background { get; set; }
    public DateTimeOffset? EventDate { get; set; }
    public string? BrideName { get; set; }
    public string? GroomName { get; set; }
    public string? Location { get; set; }
    public string? Photographer { get; set; }
    public bool IsPublic { get; set; } = true;

    public ICollection<AlbumMediaResult> AlbumMedias { get; set; } = new List<AlbumMediaResult>();
}