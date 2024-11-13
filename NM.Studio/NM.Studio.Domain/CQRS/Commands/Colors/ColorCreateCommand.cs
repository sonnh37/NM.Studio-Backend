using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.Colors;

public class ColorCreateCommand : CreateCommand
{
    public string? Name { get; set; }
}