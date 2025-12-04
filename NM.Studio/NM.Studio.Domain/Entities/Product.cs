using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public enum ProductStatus
{
    Active,
    Draft,
    Archived
}

[Index(nameof(Slug), IsUnique = true)]
[Index(nameof(Sku), IsUnique = true)]
public class Product : BaseEntity
{
    [MaxLength(64)] public string? Sku { get; set; }
    [Required] [MaxLength(256)] public string? Name { get; set; }
    [MaxLength(256)] public string? Slug { get; set; }

    public Guid? CategoryId { get; set; }
    public Guid? SubCategoryId { get; set; }
    public Guid? ThumbnailId { get; set; }
    public string? Description { get; set; }
    public string? Material { get; set; }
    public ProductStatus Status { get; set; }
    public virtual Category? Category { get; set; }
    public virtual SubCategory? SubCategory { get; set; }
    public virtual MediaBase? Thumbnail { get; set; }
    public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
}