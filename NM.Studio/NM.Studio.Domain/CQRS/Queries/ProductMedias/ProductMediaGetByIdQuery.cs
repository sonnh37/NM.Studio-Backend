using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.ProductMedias;

public class ProductMediaGetByIdQuery : GetByIdQuery
{
    public Guid? ProductId { get; set; }

    public Guid? MediaFileId { get; set; }
}