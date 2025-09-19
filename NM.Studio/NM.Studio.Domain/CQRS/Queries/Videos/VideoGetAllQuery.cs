using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.CQRS.Queries.Videos;

public class VideoGetAllQuery : GetAllQuery
{
    public int Duration { get; set; }
    public string? Description { get; set; }
    public VideoCategory Category { get; set; }
    public string? Resolution { get; set; }
    public Guid? MediaBaseId { get; set; }
    public Guid? MediaUrlId { get; set; }
}