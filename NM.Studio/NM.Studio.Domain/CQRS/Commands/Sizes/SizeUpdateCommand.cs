using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.Sizes;

public class SizeUpdateCommand : UpdateCommand
{
    public string? Name { get; set; }
}