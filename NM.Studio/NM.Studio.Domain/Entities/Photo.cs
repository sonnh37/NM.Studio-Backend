using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class Photo : BaseEntity
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Src { get; set; }

    public string? Href { get; set; }

    public string? Tag { get; set; }

    public virtual ICollection<AlbumXPhoto> AlbumsXPhotos { get; set; } = new List<AlbumXPhoto>();

    public virtual ICollection<ProductXPhoto> ProductXPhotos { get; set; } = new List<ProductXPhoto>();
}