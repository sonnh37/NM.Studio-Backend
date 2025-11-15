using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class HomeSlide : BaseEntity
{
    public Guid SlideId { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public MediaBase? Slide { get; set; }
}