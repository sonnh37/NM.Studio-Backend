using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.AlbumImages;

public class AlbumImageCreateCommand : CreateCommand
{
    public int SortOrder { get; set; }
    public bool IsCover { get; set; }
    public bool IsThumbnail { get; set; }
    public Guid? ImageId { get; set; }
    public Guid? AlbumId { get; set; }
}