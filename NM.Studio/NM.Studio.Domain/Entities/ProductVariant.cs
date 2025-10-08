using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

[Index(nameof(ProductId), nameof(Color), nameof(Size), IsUnique = true)]
[Index(nameof(Sku), IsUnique = true)]
public class ProductVariant : BaseEntity
{
    public Guid? ProductId { get; set; }
    [MaxLength(64)] public string? Sku { get; set; }
    [MaxLength(64)] public string? Color { get; set; }
    [MaxLength(32)] public string? Size { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? Price { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? RentalPrice { get; set; }
    [Column(TypeName = "decimal(18,2)")] public decimal? Deposit { get; set; }
    public int StockQuantity { get; set; }
    public ProductStatus Status { get; set; }
    public virtual Product? Product { get; set; }
    public virtual ICollection<ProductMedia> ProductMedias { get; set; } = new List<ProductMedia>();
}