using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.Sizes;

public class SizeCreateCommand : CreateCommand
{
    public string? Name { get; set; }
}