using NM.Studio.Domain.Models.CQRS.Queries.Base;

namespace NM.Studio.Domain.Models.CQRS.Queries.Albums;

public class AlbumGetBySlugQuery : BaseQuery
{
    public string Slug { get; set; }
}