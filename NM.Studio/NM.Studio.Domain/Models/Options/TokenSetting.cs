namespace NM.Studio.Domain.Models.Options;

public class TokenSetting
{
    public int AccessTokenExpiryMinutes { get; set; }
    public int RefreshTokenExpiryDays { get; set; }
}