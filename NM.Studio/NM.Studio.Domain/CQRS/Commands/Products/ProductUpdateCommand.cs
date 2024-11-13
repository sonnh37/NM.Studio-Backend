using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.Products;

public class ProductUpdateCommand : UpdateCommand
{
    public string? Sku { get; set; }

    public Guid? SubCategoryId { get; set; }
    
    public Guid? SizeId { get; set; }

    public Guid? ColorId { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }
}