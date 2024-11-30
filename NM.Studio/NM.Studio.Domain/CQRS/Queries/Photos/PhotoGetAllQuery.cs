using NM.Studio.Domain.CQRS.Queries.AlbumXPhotos;
using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Domain.CQRS.Queries.Photos;

public class PhotoGetAllQuery : GetAllQuery
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Src { get; set; }
    
    public bool? IsFeatured { get; set; }

    public string? Href { get; set; }

    public string? Tag { get; set; }

    public Guid? AlbumId {
        get;
        set;
    }
    
    public Guid? ProductId {
        get;
        set;
    }

    // public virtual ICollection<ProductXPhoto> ProductXPhotos { get; set; } = new List<ProductXPhoto>();
}