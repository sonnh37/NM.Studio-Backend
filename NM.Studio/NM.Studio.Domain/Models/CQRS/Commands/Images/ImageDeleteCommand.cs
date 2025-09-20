using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.CQRS.Commands.Images;

public class ImageDeleteCommand : DeleteCommand
{
    public string? Description { get; set; }
    public Guid? MediaBaseId { get; set; }
    public Guid? MediaUrlId { get; set; }
    public virtual MediaBase? MediaBase { get; set; }
    public virtual MediaUrl? MediaUrl { get; set; }
}