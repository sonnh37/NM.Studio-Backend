namespace NM.Studio.Domain.Models;

public class RefreshTokenResult
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public double ExpireTime { get; set; }
    public UserContextResponse? User { get; set; }
}