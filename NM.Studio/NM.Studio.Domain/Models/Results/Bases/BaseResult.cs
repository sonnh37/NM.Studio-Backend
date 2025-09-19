namespace NM.Studio.Domain.Models.Results.Bases;

public abstract class BaseResult
{
    public Guid Id { get; set; }
    public DateTimeOffset? CreatedDate { get; set; }
    public bool IsDeleted { get; set; }
}