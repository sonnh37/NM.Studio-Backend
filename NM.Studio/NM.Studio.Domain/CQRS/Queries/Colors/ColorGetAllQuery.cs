using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.Colors;

public class ColorGetAllQuery : GetAllQuery
{
    public string? Name { get; set; }

    public Guid? ProductId {
        get;
        set;
    }
}