using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class AlbumMedia : BaseEntity
{
    public Guid? AlbumId { get; set; }

    public Guid? MediaFileId { get; set; }

    public virtual Album? Album { get; set; }

    public virtual MediaFile? MediaFile { get; set; }
}