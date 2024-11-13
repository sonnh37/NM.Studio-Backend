using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.Categories;

public class CategoryUpdateCommand : UpdateCommand
{
    public string? Name { get; set; }
}