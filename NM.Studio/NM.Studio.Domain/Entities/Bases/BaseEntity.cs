namespace NM.Studio.Domain.Entities.Bases;

public abstract class BaseEntity
{
    public Guid Id { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? CreatedDate { get; set; }

    public string? LastUpdatedBy { get; set; }

    public DateTimeOffset? LastUpdatedDate { get; set; }

    public bool IsDeleted { get; set; }
}