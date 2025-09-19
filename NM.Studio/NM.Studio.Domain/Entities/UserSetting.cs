using NM.Studio.Domain.Entities.Bases;

namespace NM.Studio.Domain.Entities;

public class UserSetting : BaseEntity
{
    public Guid? UserId { get; set; }
    public string? Key { get; set; } 
    public string? Value { get; set; }
    public virtual User? User { get; set; }
}