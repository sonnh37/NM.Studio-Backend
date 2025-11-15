using NM.Studio.Domain.Models.CQRS.Queries.Base;

namespace NM.Studio.Domain.Models.CQRS.Queries.MediaBases;

public class MediaBaseGetAllQuery : GetAllQuery
{
    public string? Description { get; set; }
    public Guid? MediaBaseId { get; set; }
    public Guid? MediaUrlId { get; set; }
}