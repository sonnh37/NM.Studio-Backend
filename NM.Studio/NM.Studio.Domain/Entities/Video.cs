using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class Video : BaseEntity
{
    public int Duration { get; set; }
    public string? Description { get; set; }
    public VideoCategory Category { get; set; }
    public string? Resolution { get; set; }
    public Guid? MediaBaseId { get; set; }
    public Guid? MediaUrlId { get; set; }
    public virtual MediaBase? MediaBase { get; set; }
    public virtual MediaUrl? MediaUrl { get; set; }
}

public enum VideoCategory
{
    PreWedding,
    FullWeddingDay,
    BehindTheScenes
}