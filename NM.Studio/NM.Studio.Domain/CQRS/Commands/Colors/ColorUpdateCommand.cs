using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.Colors;

public class ColorUpdateCommand : UpdateCommand
{
    public string? Name { get; set; }
}