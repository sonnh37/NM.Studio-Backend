using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.RefreshTokens;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Queries.Auths;
using NM.Studio.Domain.CQRS.Queries.Users;
using NM.Studio.Domain.Models;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;

namespace NM.Studio.Services;

public class AuthService : IAuthService
{
    private readonly string _clientId;
    private readonly IConfiguration _configuration;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly IMapper _mapper;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IRefreshTokenService _refreshTokenService;
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
        _refreshTokenService = refreshTokenService;
        _refreshTokenRepository = _unitOfWork.RefreshTokenRepository;
    }

    public async Task<RSA> GetRSAKeyFromTokenAsync(string token, string kid)
    {
        // Bước 1: Đọc token và check refreshToken entity bằng user Id và kid
        // * vì sao không check bằng token do token ở đây thường là accessToken nên ko theer check 
        // * do là db đang lưu refreshToken
        // * nên sử dụng kid ở trong token do nó được tạo chung bởi rsa
        var userId = GetUserIdFromToken(token);

        var refreshTokenEntity = await _refreshTokenRepository.GetByUserIdAndKeyIdAsync(Guid.Parse(userId), kid);
        if (refreshTokenEntity == null) throw new Exception("RefreshToken entity not found");

        // Bước 2: Xác thực chữ ký JWT bằng public key
        var isValid = ValidateJwtSignature(refreshTokenEntity.Token, refreshTokenEntity.PublicKey);
        if (!isValid) throw new Exception("Invalid refresh token signature.");

        // Bước 3: Check ipAddress trùng với db 
        var ipAddress = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                        ?? _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";

        ipAddress = NormalizeIpAddress(ipAddress);

        if (refreshTokenEntity.IpAddress != ipAddress) throw new Exception("Ip not matched");

        return LoadRSAFromXml(refreshTokenEntity.PublicKey);
    }


    public BusinessResult Login(AuthQuery query)
    {
        var user = _userRepository.FindUsernameOrEmail(query.Account).Result;
        if (user == null)
            return BusinessResult.Fail(Const.NOT_FOUND_MSG);

        if (!BCrypt.Net.BCrypt.Verify(query.Password, user.Password))
            return BusinessResult.Fail("The password does not match.");

        var result = _mapper.Map<UserResult>(user);

        // Tạo cặp khóa RSA
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            try
            {
                var publicKey = rsa.ToXmlString(false);

                var kid = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(publicKey)));

                var accessToken = CreateToken(result, rsa, "AccessToken", kid);

                var refreshTokenValue = CreateToken(result, rsa, "RefreshToken", kid);

                var refreshTokenCreateCommand = new RefreshTokenCreateCommand
                {
                    UserId = user.Id,
                    Token = refreshTokenValue,
                    PublicKey = publicKey,
                    KeyId = kid
                };

                var res = _refreshTokenService.CreateOrUpdate<RefreshTokenResult>(refreshTokenCreateCommand).Result;

                if (!res.IsSuccess)
                    return BusinessResult.Fail("Error while saving refresh token.");

                var refreshToken = res.Data as RefreshTokenResult;

                SaveHttpOnlyCookie(accessToken, refreshToken?.Token);

                return BusinessResult.Success("Login Successfully.");
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
        if (!businessResult.IsSuccess) return businessResult;

        #endregion

        #region CheckAccessToken

        var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["accessToken"];
        if (accessToken != null)
        {
            var br = GetUserByClaims().Result;

            return br;
        }

        #endregion

        #region SaveRefreshToken

        businessResult = RefreshToken(new UserRefreshTokenCommand
        {
            RefreshToken = refreshToken
        }).Result;

        if (!businessResult.IsSuccess) return businessResult;

        #endregion

        #region CheckRefreshToken is valid => return user

        var tokenResult = businessResult.Data as TokenResult;
        businessResult = GetUserByToken(tokenResult.AccessToken).Result;

        return businessResult;

        #endregion
    }

    public async Task<BusinessResult> RefreshToken(UserRefreshTokenCommand request)
    {
        var refreshTokenEntity = await _refreshTokenRepository.GetByRefreshTokenAsync(request.RefreshToken);
        if (refreshTokenEntity == null)
            return BusinessResult.Fail("Refresh token not found.");

        var isValid = ValidateJwtSignature(request.RefreshToken, refreshTokenEntity.PublicKey);
        if (!isValid)
            return BusinessResult.Fail("Invalid refresh token signature.");

        var businessResult = _refreshTokenService.ValidateRefreshTokenIpMatch();

        if (!businessResult.IsSuccess)
            return BusinessResult.Fail("IP address mismatch.");

        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            try
            {
                var newPublicKey = rsa.ToXmlString(false);

                var user = await _userRepository.GetById(refreshTokenEntity.UserId!.Value);
                var userResult = _mapper.Map<UserResult>(user);
                var kid = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(newPublicKey)));

                var newAccessToken = CreateToken(userResult, rsa, "AccessToken", kid);

                var newRefreshToken = CreateToken(userResult, rsa, "RefreshToken", kid);

                refreshTokenEntity.Token = newRefreshToken;
                refreshTokenEntity.PublicKey = newPublicKey;
                refreshTokenEntity.KeyId = kid;
                refreshTokenEntity.Expiry = DateTimeOffset.UtcNow.AddDays(_tokenSetting.RefreshTokenExpiryDays);
                _refreshTokenRepository.Update(refreshTokenEntity);
                var isSaveChanges = await _unitOfWork.SaveChanges();
                if (!isSaveChanges)
                    return BusinessResult.Fail("Refresh token validation failed when saving changes.");

                // Bước 8: Lưu access token vào cookie (nếu cần)
                SaveHttpOnlyCookie(newAccessToken, newRefreshToken);

                // Bước 9: Trả về access token và refresh token mới
                var tokenResult = new TokenResult
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                };

                return BusinessResult.Success(tokenResult, "Token refreshed successfully.");
            }
            finally
            {
                rsa.PersistKeyInCsp = false; // Đảm bảo xóa key từ container
            }
        }
    }

    public async Task<BusinessResult> Logout(UserLogoutCommand userLogoutCommand)
    {
        try
        {
            var userRefreshToken = await _refreshTokenRepository
                .GetByRefreshTokenAsync(userLogoutCommand.RefreshToken ?? string.Empty);
            if (userRefreshToken == null)
                return BusinessResult.Fail("You are not logged in, please log in to continue.");
            _refreshTokenRepository.Delete(userRefreshToken, true);

            var isSaved = await _unitOfWork.SaveChanges();
            if (!isSaved) throw new Exception();

            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("accessToken");
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");

            return BusinessResult.Fail("The account has been logged out.");
        }
        catch (Exception e)
        {
            return BusinessResult.ExceptionError(e.Message);
        }
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
            return BusinessResult.Fail("No access token provided");

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(accessToken);

        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        if (string.IsNullOrEmpty(userId))
            return BusinessResult.Fail("Error the access token provided");
        

        var businessResult = await _userService.GetById<UserResult>(new UserGetByIdQuery{Id = Guid.Parse(userId)});

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
            return BusinessResult.Fail("You are not logged in, please log in to continue.");

        var businessResult = _refreshTokenService.ValidateRefreshTokenIpMatch();
        return businessResult;
    }

    public async Task<BusinessResult> GetUserByClaims()
    {
        try
        {
            // Lấy thông tin UserId từ Claims
            var userId = GetUserIdFromClaims();
            if (!userId.HasValue)
                return BusinessResult.Fail("No user found.");

            // Lấy thông tin người dùng từ database
            var userResult = await _userService.GetById<UserResult>(new UserGetByIdQuery{Id = userId.Value});
            return userResult;
        }
        catch (Exception ex)
        {
            return BusinessResult.ExceptionError(ex.Message);
        }
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
            new("TokenType", tokenType) // 🟢 Thêm claim để phân biệt
        };

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