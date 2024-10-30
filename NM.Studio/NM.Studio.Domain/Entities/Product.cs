using NM.Studio.Domain.Entities.Bases;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.Entities;

public class Product : BaseEntity
{
    public string? Sku { get; set; }

    public Guid? CategoryId { get; set; }
    
    public Guid? SubCategoryId { get; set; }

    public Guid? SizeId { get; set; }

    public Guid? ColorId { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public virtual Color? Color { get; set; }

    public virtual Category? Category { get; set; }
    
    public virtual SubCategory? SubCategory { get; set; }

    public virtual Size? Size { get; set; }

    public ProductStatus Status { get; set; }

    public virtual ICollection<ProductXPhoto> ProductXPhotos { get; set; } = new List<ProductXPhoto>();
}