using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Commands.UserTokens;
using NM.Studio.Domain.CQRS.Queries.Auths;
using NM.Studio.Domain.CQRS.Queries.Users;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models;
using NM.Studio.Domain.Models.Options;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;

namespace NM.Studio.Services;

public class AuthService : IAuthService
{
    private readonly string _clientId;
    private readonly IConfiguration _configuration;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly IMapper _mapper;
    private readonly IUserTokenRepository _userTokenRepository;
    private readonly IRefreshTokenService _userTokenService;
    protected readonly TokenSetting _tokenSetting;
    protected readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;

    public AuthService(IMapper mapper,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        IUserService userService,
        IOptions<TokenSetting> tokenSetting,
        IRefreshTokenService refreshTokenService
    )
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _httpContextAccessor ??= new HttpContextAccessor();
        _tokenSetting = tokenSetting.Value;
        _userRepository = _unitOfWork.UserRepository;
        _userService = userService;
        _userTokenService = refreshTokenService;
        _userTokenRepository = _unitOfWork.UserTokenRepository;
    }

    public async Task<RSA> GetRSAKeyFromTokenAsync(string token, string kid)
    {
        // Bước 1: Đọc token và check refreshToken entity bằng user Id và kid
        // * vì sao không check bằng token do token ở đây thường là accessToken nên ko theer check 
        // * do là db đang lưu refreshToken
        // * nên sử dụng kid ở trong token do nó được tạo chung bởi rsa
        var userId = GetUserIdFromToken(token);

        var refreshTokenEntity = await _userTokenRepository.GetByUserIdAndKeyIdAsync(Guid.Parse(userId), kid);
        if (refreshTokenEntity == null) throw new Exception("RefreshToken entity not found");

        // Bước 2: Xác thực chữ ký JWT bằng public key
        var isValid = ValidateJwtSignature(token, refreshTokenEntity.PublicKey);
        if (!isValid) throw new Exception("Invalid refresh token signature.");

        // Bước 3: Check ipAddress trùng với db 
        var ipAddress = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                        ?? _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";

        ipAddress = NormalizeIpAddress(ipAddress);

        if (refreshTokenEntity.IpAddress != ipAddress) throw new Exception("Ip not matched");

        return LoadRSAFromXml(refreshTokenEntity.PublicKey);
    }


    public async Task<BusinessResult> Login(AuthQuery query)
    {
        var user = await _userRepository.FindUsernameOrEmail(query.Account);
        if (user == null)
            throw new NotFoundException(Const.NOT_FOUND_MSG);

        if (!BCrypt.Net.BCrypt.Verify(query.Password, user.Password))
            throw new DomainException("The password does not match.");

        var result = _mapper.Map<UserResult>(user);

        // Tạo cặp khóa RSA
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            try
            {
                var publicKey = rsa.ToXmlString(false);
                var privateKey = rsa.ToXmlString(true);

                var kid = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(privateKey)));

                var accessToken = CreateToken(result, rsa, "AccessToken", kid);

                var refreshTokenValue = GenerateRefreshToken();

                var refreshTokenCreateCommand = new UserTokenCreateCommand
                {
                    UserId = user.Id,
                    Token = refreshTokenValue,
                    PublicKey = publicKey,
                    KeyId = kid
                };

                var res = _userTokenService.CreateOrUpdate(refreshTokenCreateCommand).Result;

                if (res.Status != Status.OK)
                    return res;

                var refreshToken = res.Data as UserTokenResult;

                SaveHttpOnlyCookie(accessToken, refreshToken?.Token);


                return new BusinessResult
                {
                    Message = "Login Successfully."
                };
            }
            finally
            {
                rsa.PersistKeyInCsp = false;
            }
        }
    }

    public BusinessResult GetUserByCookie(AuthGetByCookieQuery request)
    {
        BusinessResult? businessResult = null;

        #region Check refresh token

        var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
        businessResult = ValidateRefreshTokenIpAdMatch(refreshToken);
        if (businessResult.Status != Status.OK) return businessResult;

        #endregion

        #region CheckAccessToken

        var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["accessToken"];
        if (accessToken != null)
        {
            var user = GetUserByClaims().Result;

            return new BusinessResult(user);
        }

        #endregion

        #region SaveRefreshToken

        businessResult = RefreshToken(new UserRefreshTokenCommand
        {
            RefreshToken = refreshToken
        }).Result;

        if (businessResult.Status != Status.OK) return businessResult;

        #endregion

        #region CheckRefreshToken is valid => return user

        var tokenResult = businessResult.Data as TokenResult;
        businessResult = GetUserByToken(tokenResult.AccessToken).Result;

        return businessResult;

        #endregion
    }

    public async Task<BusinessResult> RefreshToken(UserRefreshTokenCommand request)
    {
        var refreshTokenEntity = await _userTokenRepository.GetQueryable(x =>
                x.Token != null && request.RefreshToken != null && x.Token.ToLower() == request.RefreshToken.ToLower())
            .SingleOrDefaultAsync();

        if (refreshTokenEntity == null || refreshTokenEntity.Token != request.RefreshToken || refreshTokenEntity.Expiry <= DateTimeOffset.UtcNow)
        {
            throw new UnauthorizedException("Invalid refresh token");
        }
        
        var businessResult = _userTokenService.ValidateRefreshTokenIpMatch();
        if (businessResult.Status != Status.OK)
            return businessResult;

        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            try
            {
                var newPublicKey = rsa.ToXmlString(false);
                var newPrivateKey = rsa.ToXmlString(true);

                var user = await _userRepository.GetQueryable(m => m.Id == refreshTokenEntity.UserId)
                    .SingleOrDefaultAsync();
                var userResult = _mapper.Map<UserResult>(user);
                var kid = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(newPrivateKey)));

                var newAccessToken = CreateToken(userResult, rsa, "AccessToken", kid);
                var newRefreshToken = GenerateRefreshToken();

                refreshTokenEntity.Token = newRefreshToken;
                refreshTokenEntity.PublicKey = newPublicKey;
                refreshTokenEntity.KeyId = kid;
                refreshTokenEntity.Expiry = DateTimeOffset.UtcNow.AddDays(_tokenSetting.RefreshTokenExpiryDays);
                _userTokenRepository.Update(refreshTokenEntity);

                var isSaveChanges = await _unitOfWork.SaveChanges();
                if (!isSaveChanges)
                    throw new Exception("Failed to update refresh token.");

                SaveHttpOnlyCookie(newAccessToken, newRefreshToken);

                var tokenResult = new TokenResult
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                };

                return new BusinessResult(tokenResult, "Token refreshed successfully.");
            }
            finally
            {
                rsa.PersistKeyInCsp = false;
            }
        }
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

    public async Task<BusinessResult> Logout(UserLogoutCommand userLogoutCommand)
    {
        var userRefreshToken = await _userTokenRepository
            .GetByRefreshTokenAsync(userLogoutCommand.RefreshToken ?? string.Empty);
        if (userRefreshToken == null)
            throw new DomainException("You are not logged in, please log in to continue.");
        _userTokenRepository.Delete(userRefreshToken, true);

        var isSaved = await _unitOfWork.SaveChanges();
        if (!isSaved) throw new Exception();

        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("accessToken");
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");

        throw new DomainException("The account has been logged out.");
    }


    public Task<BusinessResult> RegisterByGoogleAsync(UserCreateByGoogleTokenCommand request)
    {
        throw new NotImplementedException();
    }

    public Task<BusinessResult> LoginByGoogleTokenAsync(VerifyGoogleTokenRequest request)
    {
        throw new NotImplementedException();
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

    public string GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        return jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value; // "sub" thường là userId
    }

    private string GetKidFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        return jwtToken.Header.Kid; // Lấy Key ID (kid)
    }

    private RSA LoadRSAFromXml(string xmlKey)
    {
        var rsa = RSA.Create();
        rsa.FromXmlString(xmlKey);
        return rsa;
    }

    // private async Task<BusinessResult> SaveRefreshToken(Guid userId, string refreshToken)
    // {
    //     var user = await _userRepository.GetById(userId);
    //     var userResult = _mapper.Map<UserResult>(user);
    //
    //     // Create new access token
    //     var accessToken = CreateToken(userResult);
    //
    //     var accessTokenOptions = new CookieOptions
    //     {
    //         HttpOnly = true,
    //         Secure = true,
    //         SameSite = SameSiteMode.None,
    //         Expires = DateTimeOffset.UtcNow.AddMinutes(_tokenSetting.AccessTokenExpiryMinutes),
    //     };
    //
    //     _httpContextAccessor.HttpContext.Response.Cookies.Append("accessToken", accessToken, accessTokenOptions);
    //
    //     var tokenResult = new TokenResult { AccessToken = accessToken, RefreshToken = refreshToken };
    //     return new ResponseBuilder<TokenResult>()
    //         .WithData(tokenResult)
    //         .WithStatus(Const.SUCCESS_CODE)
    //         .WithMessage(Const.SUCCESS_LOGIN_MSG)
    //         .Build();
    // }

    protected async Task<BusinessResult> GetUserByToken(string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
            throw new DomainException("No access token provided");

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(accessToken);

        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        if (string.IsNullOrEmpty(userId))
            throw new DomainException("Error the access token provided");

        var businessResult = await _userService.GetById(new UserGetByIdQuery { Id = Guid.Parse(userId) });

        return businessResult;
    }

    private bool ValidateJwtSignature(string token, string publicKey)
    {
        try
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(rsa),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, validationParameters, out _);

            return true;
        }
        catch (SecurityTokenSignatureKeyNotFoundException)
        {
            // Chữ ký không hợp lệ hoặc không tìm thấy key
            Console.WriteLine("Invalid signature or key not found.");
            return false;
        }
        catch (SecurityTokenExpiredException)
        {
            // Token hết hạn
            Console.WriteLine("Token has expired.");
            return false;
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            // Chữ ký không hợp lệ
            Console.WriteLine("Invalid token signature.");
            return false;
        }
        catch (Exception ex)
        {
            // Các lỗi khác
            Console.WriteLine($"Token validation failed: {ex.Message}");
            return false;
        }
    }


    public BusinessResult ValidateRefreshTokenIpAdMatch(string refreshToken)
    {
        if (refreshToken == null)
            throw new DomainException("You are not logged in, please log in to continue.");

        var businessResult = _userTokenService.ValidateRefreshTokenIpMatch();
        return businessResult;
    }

    private async Task<User?> GetUserByClaims()
    {
        // Lấy thông tin UserId từ Claims
        var userId = GetUserIdFromClaims();
        if (!userId.HasValue)
            throw new NotFoundException("No user found.");

        // Lấy thông tin người dùng từ database
        var userResult = await _userRepository.GetQueryable(m => m.Id == userId.Value).SingleOrDefaultAsync();
        return userResult;
    }

    private Guid? GetUserIdFromClaims()
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("Id")?.Value;
        if (string.IsNullOrEmpty(userIdClaim)) return null;

        return Guid.Parse(userIdClaim);
    }

    private string CreateToken(UserResult user, RSACryptoServiceProvider rsa, string tokenType, string kid)
    {
        var claims = new List<Claim>
        {
            new("Id", user.Id.ToString()),
            new("Role", user.Role.ToString()),
            new("TokenType", tokenType) 
        };
        
         // rsa.FromXmlString(privateKey);

        var key = new RsaSecurityKey(rsa)
        {
            KeyId = kid
        };
        var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: tokenType == "AccessToken"
                ? DateTimeOffset.Now.AddMinutes(_tokenSetting.AccessTokenExpiryMinutes).UtcDateTime
                : DateTimeOffset.Now.AddDays(_tokenSetting.RefreshTokenExpiryDays).UtcDateTime,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private void SaveHttpOnlyCookie(string accessToken, string refreshToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var accessTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddMinutes(_tokenSetting.AccessTokenExpiryMinutes)
        };

        var refreshTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddDays(_tokenSetting.RefreshTokenExpiryDays)
        };

        httpContext.Response.Cookies.Append("accessToken", accessToken, accessTokenOptions);
        httpContext.Response.Cookies.Append("refreshToken", refreshToken, refreshTokenOptions);
    }
}