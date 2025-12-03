using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.MediaBases;

public class MediaBaseUpdateCommand : UpdateCommand
{
    public string? DisplayName { get; set; }
    public string? Title { get; set; }
    public string? Format { get; set; }
    public long Size { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string? CreatedMediaBy { get; set; }
    public DateTimeOffset? TakenMediaDate { get; set; }
    public string? MediaUrl { get; set; }
    public ResourceType? ResourceType { get; set; }
}