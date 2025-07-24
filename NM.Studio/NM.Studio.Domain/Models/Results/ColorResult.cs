using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class ColorResult : BaseResult
{
    public string? Name { get; set; }

    public List<ProductColorResult> ProductColors { get; set; } = new();
}