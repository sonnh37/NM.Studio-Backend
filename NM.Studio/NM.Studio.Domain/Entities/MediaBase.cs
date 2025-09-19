using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class MediaBase : BaseEntity
{
    public string? DisplayName { get; set; }
    public string? Title { get; set; }
    public string? MimeType { get; set; }
    public long Size { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string? CreatedMediaBy { get; set; }
    public DateTimeOffset? TakenMediaDate { get; set; }
    public virtual Image? Image { get; set; }
    public virtual Video? Video { get; set; }
}