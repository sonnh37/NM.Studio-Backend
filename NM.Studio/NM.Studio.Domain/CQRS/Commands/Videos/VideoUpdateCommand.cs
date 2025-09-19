using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.CQRS.Commands.Videos;

public class VideoUpdateCommand : UpdateCommand
{
    public int Duration { get; set; }
    public string? Description { get; set; }
    public VideoCategory Category { get; set; }
    public string? Resolution { get; set; }
    public Guid? MediaBaseId { get; set; }
    public Guid? MediaUrlId { get; set; }
}