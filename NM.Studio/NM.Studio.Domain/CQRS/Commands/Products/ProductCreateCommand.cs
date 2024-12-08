using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.CQRS.Commands.Products;

public class ProductCreateCommand : CreateCommand
{
    public string? Sku { get; set; }

    public Guid? SubCategoryId { get; set; }
    
    public string? Slug { get; set; }

    public Guid? SizeId { get; set; }

    public Guid? ColorId { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }
    
    public ProductStatus Status { get; set; }
}