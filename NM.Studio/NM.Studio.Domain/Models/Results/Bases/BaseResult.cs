namespace NM.Studio.Domain.Models.Results.Bases;

public abstract class BaseResult
{
    public Guid Id { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? CreatedDate { get; set; }

    public string? LastUpdatedBy { get; set; }

    public DateTimeOffset? LastUpdatedDate { get; set; }

    public bool IsDeleted { get; set; }
}