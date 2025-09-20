using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.Images;

public class ImageCreateCommand : CreateCommand
{
    public string? Description { get; set; }
    public Guid? MediaBaseId { get; set; }
    public Guid? MediaUrlId { get; set; }
}