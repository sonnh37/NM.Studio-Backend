using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.Sizes;

public class SizeGetAllQuery : GetAllQuery
{
    public string? Name { get; set; } // e.g., "S", "M", "L", "XL"
    public string? DisplayName { get; set; } // e.g., "Small", "Medium", "Large"
    public string? Description { get; set; } // Additional size details

    // Measurements (in centimeters)
    public decimal? Bust { get; set; } // Chest measurement
    public decimal? Waist { get; set; } // Waist measurement
    public decimal? Hip { get; set; } // Hip measurement
    public decimal? Length { get; set; } // Length measurement

    public string? SizeGuide { get; set; } // Size guide notes
    public bool? IsActive { get; set; } // Availability flag
    public int? SortOrder { get; set; } // Display order

    public Guid? ProductId { get; set; }
}