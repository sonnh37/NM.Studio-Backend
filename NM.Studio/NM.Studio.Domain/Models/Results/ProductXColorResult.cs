using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class ProductColorResult : BaseResult
{
    public Guid? ProductId { get; set; }

    public Guid? ColorId { get; set; }

    public bool IsActive { get; set; }

    public ProductResult? Product { get; set; }

    public ColorResult? Color { get; set; }
}