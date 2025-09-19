using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class MediaUrl : BaseEntity
{
    public string? UrlInternal { get; set; }
    public string? UrlExternal { get; set; }

    public virtual Image? Image { get; set; }
    public virtual Video? Video { get; set; }
}