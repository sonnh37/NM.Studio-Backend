using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class UserRefreshToken : BaseEntity
{
    public Guid UserId { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime ExpirationDate { get; set; }
    public virtual User? User { get; set; }
}