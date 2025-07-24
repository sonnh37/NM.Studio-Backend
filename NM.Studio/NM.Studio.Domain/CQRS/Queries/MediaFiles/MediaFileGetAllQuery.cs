using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.MediaFiles;

public class MediaFileGetAllQuery : GetAllQuery
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Src { get; set; }

    public bool? IsFeatured { get; set; }

    public string? Href { get; set; }

    public string? Tag { get; set; }

    public Guid? AlbumId { get; set; }

    public Guid? ProductId { get; set; }

    // public virtual ICollection<ProductMedia> ProductMedias { get; set; } = new List<ProductMedia>();
}