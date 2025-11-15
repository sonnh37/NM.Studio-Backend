namespace NM.Studio.Domain.Models.Options;

public class UserJwtOptions
{
    public string ValidAudience { get; set; }
    public string ValidIssuer { get; set; }
    public string PrivateKey { get; set; }
    public string PublicKey { get; set; }
    public double ExpiredSecond { get; set; }
    public int RefreshTokenExpiryInDays { get; set; }
}