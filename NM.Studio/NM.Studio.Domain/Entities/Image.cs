using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class Image : BaseEntity
{
    public string? Description { get; set; }
    public Guid? MediaBaseId { get; set; }
    public Guid? MediaUrlId { get; set; }
    public virtual MediaBase? MediaBase { get; set; }
    public virtual MediaUrl? MediaUrl { get; set; }
}