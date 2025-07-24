using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.ProductSizes;

public class ProductSizeGetAllQuery : GetAllQuery
{
    public Guid? ProductId { get; set; }

    public Guid? SizeId { get; set; }

    public bool? IsActive { get; set; }
}