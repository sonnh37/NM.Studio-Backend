namespace NM.Studio.Domain.Models;

public class TokenResult
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public double ExpireTime { get; set; }
}