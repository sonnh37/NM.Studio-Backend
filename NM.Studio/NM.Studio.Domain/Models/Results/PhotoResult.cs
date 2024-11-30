using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class PhotoResult : BaseResult
{
    public string? Title { get; set; }

    public string? Description { get; set; }
    
    public bool IsFeatured { get; set; }

    public string? Type { get; set; }

    public string? Src { get; set; }

    public string? Href { get; set; }

    public Guid? AlbumId { get; set; }

    public Guid? ProductId { get; set; }

    public List<AlbumXPhotoResult> AlbumsXPhotos { get; set; } = new();

    public List<ProductXPhotoResult> ProductXPhotos { get; set; } = new();
}