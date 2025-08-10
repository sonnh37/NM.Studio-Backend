using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.CQRS.Commands.Products;

public class ProductUpdateCommand : UpdateCommand
{
    public string? Sku { get; set; }
    public string? Slug { get; set; }
    public string? Name { get; set; }

    // Categorization
    public Guid? SubCategoryId { get; set; }

    // Pricing and Availability
    public decimal? Price { get; set; }
    public decimal? RentalPrice { get; set; } // For rentable items
    public decimal? Deposit { get; set; } // For rentable items
    public bool IsRentable { get; set; }
    public bool IsSaleable { get; set; }

    // Details
    public string? Description { get; set; }
    public string? Material { get; set; }
    public string? Brand { get; set; }
    public string? Style { get; set; }
    public string? Care { get; set; } // Care instructions

    // Status
    public ProductStatus Status { get; set; }
}