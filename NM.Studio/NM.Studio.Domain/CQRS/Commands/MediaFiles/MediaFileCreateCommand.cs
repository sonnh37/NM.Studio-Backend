using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.MediaFiles;

public class MediaFileCreateCommand : CreateCommand
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool IsFeatured { get; set; }

    public string? Src { get; set; }

    public string? Href { get; set; }

    public string? Tag { get; set; }
}