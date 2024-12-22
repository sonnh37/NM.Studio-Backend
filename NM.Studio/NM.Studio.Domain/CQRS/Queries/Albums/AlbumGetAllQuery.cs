using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Domain.CQRS.Queries.Albums;

public class AlbumGetAllQuery : GetAllQuery
{
    public string? Title { get; set; }
    
    public string? Slug { get; set; }
    
    public string? Description { get; set; }

    public string? Background { get; set; }
}