namespace NM.Studio.Domain.Models;

public class TokenSetting
{
    public int AccessTokenExpiryMinutes { get; set; }
    public int RefreshTokenExpiryDays { get; set; }
}