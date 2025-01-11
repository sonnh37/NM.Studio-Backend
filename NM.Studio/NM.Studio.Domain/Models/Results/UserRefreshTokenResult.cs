using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class UserRefreshTokenResult : BaseResult
{
    public Guid UserId { get; set; }
    
    public string? RefreshToken { get; set; }
    
    public DateTime ExpirationDate { get; set; }
}