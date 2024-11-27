using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class Service : BaseEntity
{
    public string? Name { get; set; }
    
    public string? Slug { get; set; }

    public string? Description { get; set; }

    public string? Src { get; set; }

    public decimal? Price { get; set; }
}