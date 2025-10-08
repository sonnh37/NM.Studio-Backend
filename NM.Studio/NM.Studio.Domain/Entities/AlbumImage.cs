using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class AlbumImage : BaseEntity
{
    public bool IsCover { get; set; }
    public bool IsThumbnail { get; set; }
    public Guid? ImageId { get; set; }
    public Guid? AlbumId { get; set; }
    public virtual Album? Album { get; set; }
    public virtual MediaBase? Image { get; set; }
}