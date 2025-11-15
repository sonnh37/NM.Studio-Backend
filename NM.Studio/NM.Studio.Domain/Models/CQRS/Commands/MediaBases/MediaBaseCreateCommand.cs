using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.MediaBases;

public class MediaBaseCreateCommand : CreateCommand
{
    public string Src { get; set; }
}