using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.ProductColors;

public class ProductColorGetByIdQuery : GetByIdQuery
{
    public Guid? ProductId { get; set; }

    public Guid? ColorId { get; set; }

    public bool? IsActive { get; set; }
}