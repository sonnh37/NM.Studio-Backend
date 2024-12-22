using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class ProductXSizeResult : BaseResult
{
    public Guid? ProductId { get; set; }
    
    public Guid? SizeId { get; set; }
    
    public bool IsActive { get; set; }

    public ProductResult? Product { get; set; }
    
    public SizeResult? Size { get; set; }
}