using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class MediaFile : BaseEntity
{
    public string? Title { get; set; }
    public string? DisplayTitle { get; set; }
    public string? Description { get; set; }
    public string? AltText { get; set; }
    public string? Src { get; set; }
    public string? ThumbnailSrc { get; set; }
    public string? MediumSrc { get; set; }
    public string? LargeSrc { get; set; }
    public string? Href { get; set; }
    public MediaType Type { get; set; }
    public string? MimeType { get; set; }
    public long FileSize { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public TimeSpan? Duration { get; set; }
    public string? Resolution { get; set; }
    public string? Format { get; set; }
    public string? Tag { get; set; }
    public bool IsFeatured { get; set; }
    public int SortOrder { get; set; }
    public MediaCategory Category { get; set; }
    public bool IsActive { get; set; }
    public bool IsWatermarked { get; set; }
    public string? Copyright { get; set; }
    public string? CreatedMediaBy { get; set; }
    public DateTimeOffset? TakenMediaDate { get; set; }
    public string? Location { get; set; }
    public virtual ICollection<AlbumMedia> AlbumMedias { get; set; } = new List<AlbumMedia>();
    public virtual ICollection<ProductMedia> ProductMedias { get; set; } = new List<ProductMedia>();
}

public enum MediaType
{
    Image,
    Video,
    Audio,
    Document,
    Animation
}

public enum MediaCategory
{
    Product,
    Model,
    Detail,
    Marketing,
    Banner,
    Advertisement,
    Event,
    BehindTheScenes,
    Document,
    Contract,
    Lifestyle,
    Social,
    Misc
}