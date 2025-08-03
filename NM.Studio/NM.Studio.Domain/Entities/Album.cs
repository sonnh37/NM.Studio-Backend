using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class Album : BaseEntity
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

    public virtual ICollection<AlbumMedia> AlbumMedias { get; set; } = new List<AlbumMedia>();
}