using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.AlbumImages;

public class AlbumImageCreateCommand : CreateCommand
{
    
    public bool IsCover { get; set; }
    public bool IsThumbnail { get; set; }
    public Guid? ImageId { get; set; }
    public Guid? AlbumId { get; set; }
}