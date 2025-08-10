using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.Albums;

public class AlbumGetByIdQuery : GetByIdQuery
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
    public bool? IsPublic { get; set; }
}