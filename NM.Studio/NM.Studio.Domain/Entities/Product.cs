using NM.Studio.Domain.Entities.Bases;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.Entities;

public class Product : BaseEntity
{
    public string? Sku { get; set; }

    public string? Slug { get; set; }

    public Guid? SubCategoryId { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public virtual SubCategory? SubCategory { get; set; }

    public ProductStatus Status { get; set; }

    public virtual ICollection<ProductXPhoto> ProductXPhotos { get; set; } = new List<ProductXPhoto>();

    public virtual ICollection<ProductXColor> ProductXColors { get; set; } = new List<ProductXColor>();

    public virtual ICollection<ProductXSize> ProductXSizes { get; set; } = new List<ProductXSize>();
}