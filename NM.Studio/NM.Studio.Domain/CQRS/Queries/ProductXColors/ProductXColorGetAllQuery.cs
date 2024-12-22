using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.ProductXColors;

public class ProductXColorGetAllQuery : GetAllQuery
{
    public Guid? ProductId { get; set; }
    
    public Guid? ColorId { get; set; }
    
    public bool? IsActive { get; set; }
}