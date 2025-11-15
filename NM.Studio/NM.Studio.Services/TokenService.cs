using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Options;
using NM.Studio.Domain.Utilities;

namespace NM.Studio.Services;

public class TokenService : ITokenService
{
    private readonly ILogger<TokenService> _logger;
    private readonly UserJwtOptions _userJwtOptions;

    public TokenService(IOptions<UserJwtOptions> userJwtOptions, ILogger<TokenService> logger)
    {
        this._userJwtOptions = userJwtOptions.Value;
        this._logger = logger;
    }

    public string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new("Id", user.Id.ToString()),
            new("Role", user.Role?.ToString() ?? ""),
            new Claim("DisplayName", user.FullName ?? string.Empty),
            new Claim("Phone", user.Phone ?? string.Empty),
            new Claim("AvatarUrl", user.Avatar?.MediaUrl ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // if (roles != null)
        // {
        //     foreach (var role in roles)
        //     {
        //         authClaims.Add(new Claim(ClaimTypes.Role, role));
        //     }
        // }
        // if (permissionClaims != null)
        // {
        //     foreach (var claim in permissionClaims)
        //     {
        //         authClaims.Add(new Claim("Permission", claim.Value));
        //     }
        // }

        var signingCredentials = new SigningCredentials(
            new RsaSecurityKey(RsaHelper.CreateRsaFromPrivateKey(_userJwtOptions.PrivateKey)),
            SecurityAlgorithms.RsaSha256
        );

        var token = new JwtSecurityToken(
            issuer: _userJwtOptions.ValidIssuer,
            audience: _userJwtOptions.ValidAudience,
            claims: claims,
            expires: DateTime.Now.AddSeconds(_userJwtOptions.ExpiredSecond),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public bool IsTokenExpired(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken == null)
        {
            throw new SecurityTokenException("Invalid token format");
        }

        return jwtToken.ValidTo < DateTime.UtcNow;
    }

    public string GenerateKid()
    {
        var kid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        return kid;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new RsaSecurityKey(RsaHelper.CreateRsaFromPublicKey(_userJwtOptions.PublicKey)),
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null)
            throw new SecurityTokenException("Invalid token");
        return principal;
    }

    private string NormalizeIpAddress(string ipAddress)
    {
        if (ipAddress.Contains(",")) ipAddress = ipAddress.Split(',')[0].Trim();

        if (IPAddress.TryParse(ipAddress, out var ip))
        {
            if (ip.IsIPv4MappedToIPv6) return ip.MapToIPv4().ToString();

            // Chuyển loopback IPv6 (::1) về loopback IPv4 (127.0.0.1)
            if (IPAddress.IPv6Loopback.Equals(ip)) return IPAddress.Loopback.ToString(); // Trả về 127.0.0.1
        }

        return ipAddress;
    }

    private string GetKidFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        return jwtToken.Header.Kid; // Lấy Key ID (kid)
    }
}