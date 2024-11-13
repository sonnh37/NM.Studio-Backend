using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.SubCategories;

public class SubCategoryUpdateCommand : UpdateCommand
{
    public string? Name { get; set; }
    
    public Guid? CategoryId { get; set; }
}