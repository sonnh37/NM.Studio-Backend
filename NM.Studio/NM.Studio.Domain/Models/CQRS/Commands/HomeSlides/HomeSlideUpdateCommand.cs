using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.HomeSlides;

public class HomeSlideUpdateCommand : UpdateCommand
{
    public Guid SlideId { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}