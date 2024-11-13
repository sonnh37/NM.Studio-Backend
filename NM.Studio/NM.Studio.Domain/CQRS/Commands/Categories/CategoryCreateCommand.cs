using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.Categories;

public class CategoryCreateCommand : CreateCommand
{
    public string? Name { get; set; }
}