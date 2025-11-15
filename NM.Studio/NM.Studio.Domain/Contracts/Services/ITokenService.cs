using System.Security.Claims;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.Contracts.Services;

public interface ITokenService
{
    string GenerateRefreshToken();
    bool IsTokenExpired(string token);
    string GenerateJwtToken(User user);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}