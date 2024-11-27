using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.Services;

public class ServiceCreateCommand : CreateCommand
{
    public string? Name { get; set; }
    
    public string? Slug { get; set; }

    public string? Description { get; set; }

    public string? Src { get; set; }

    public decimal? Price { get; set; }

    public TimeSpan? Duration { get; set; }

    public string? Promotion { get; set; }

    public bool IsActive { get; set; }
}