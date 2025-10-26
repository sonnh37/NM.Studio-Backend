using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class HomeSlideResult : BaseResult
{
    public Guid SlideId { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public MediaBaseResult? Slide { get; set; }
}