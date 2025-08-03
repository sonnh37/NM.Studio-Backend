using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.ProductMedias;

public class ProductMediaGetAllQuery : GetAllQuery
{
    public Guid? ProductId { get; set; }

    public Guid? ColorId { get; set; }

    public bool? IsActive { get; set; }
}