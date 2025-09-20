using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Queries.Base;

namespace NM.Studio.Domain.Models.CQRS.Queries.Videos;

public class VideoGetAllQuery : GetAllQuery
{
    public int Duration { get; set; }
    public string? Description { get; set; }
    public VideoCategory Category { get; set; }
    public string? Resolution { get; set; }
    public Guid? MediaBaseId { get; set; }
    public Guid? MediaUrlId { get; set; }
}