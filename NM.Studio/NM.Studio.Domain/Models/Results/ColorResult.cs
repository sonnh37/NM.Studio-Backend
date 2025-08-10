using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class ColorResult : BaseResult
{
    public string? Name { get; set; }
    public string? ColorCode { get; set; }
    public string? ColorType { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public ICollection<ProductColorResult> ProductColors { get; set; } = new List<ProductColorResult>();
}