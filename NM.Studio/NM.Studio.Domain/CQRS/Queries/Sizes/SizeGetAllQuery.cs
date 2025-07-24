using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.Sizes;

public class SizeGetAllQuery : GetAllQuery
{
    public string? Name { get; set; }

    public Guid? ProductId { get; set; }
}