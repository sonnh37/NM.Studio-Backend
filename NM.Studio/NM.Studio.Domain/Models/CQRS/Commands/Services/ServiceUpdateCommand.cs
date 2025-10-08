using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.Services;

public class ServiceUpdateCommand : UpdateCommand
{
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public bool IsFeatured { get; set; }
    public int? HomeSortOrder { get; set; }
    
    public Guid? BackgroundCoverId { get; set; }
    public Guid? ThumbnailId { get; set; }
    public string? TermsAndConditions { get; set; }
}