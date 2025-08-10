using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.ProductSizes;

public class ProductSizeGetByIdQuery : GetByIdQuery
{
    public Guid? ProductId { get; set; }

    public Guid? SizeId { get; set; }

    public bool? IsActive { get; set; }
}