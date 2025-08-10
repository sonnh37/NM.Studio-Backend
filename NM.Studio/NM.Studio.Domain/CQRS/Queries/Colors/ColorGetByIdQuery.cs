using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.CQRS.Queries.Colors;

public class ColorGetByIdQuery : GetByIdQuery
{
    public string? Name { get; set; }
    public string? ColorCode { get; set; }
    public string? ColorType { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public bool? IsActive { get; set; }
    public int? SortOrder { get; set; }
}