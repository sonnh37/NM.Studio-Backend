﻿using System.IdentityModel.Tokens.Jwt;
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
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;

namespace NM.Studio.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly string _clientId;
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private readonly IRefreshTokenService _refreshTokenService;
    protected readonly TokenSetting _tokenSetting;

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
        var userId = GetUserIdFromToken(token);

        var refreshTokenEntity = await _refreshTokenRepository.GetByUserIdAndKeyIdAsync(Guid.Parse(userId), kid);
        if (refreshTokenEntity == null)
        {
            throw new Exception("RefreshToken entity not found");
        }
        
        var ipAddress = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                        ?? _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";

        ipAddress = NormalizeIpAddress(ipAddress);

        if (refreshTokenEntity.IpAddress != ipAddress)
        {
            throw new Exception("Ip not matched");
        }

        return LoadRSAFromXml(refreshTokenEntity.PublicKey); 
    }
    
    private string NormalizeIpAddress(string ipAddress)
    {
        if (ipAddress.Contains(","))
        {
            ipAddress = ipAddress.Split(',')[0].Trim();
        }
        
        if (IPAddress.TryParse(ipAddress, out var ip))
        {
            if (ip.IsIPv4MappedToIPv6)
            {
                return ip.MapToIPv4().ToString();
            }
            
            // Chuyển loopback IPv6 (::1) về loopback IPv4 (127.0.0.1)
            if (IPAddress.IPv6Loopback.Equals(ip))
            {
                return IPAddress.Loopback.ToString(); // Trả về 127.0.0.1
            }
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


    public BusinessResult Login(AuthQuery query)
    {
        var user = _userRepository.FindUsernameOrEmail(query.Account).Result;
        if (user == null)
            return new ResponseBuilder()
                .WithStatus(Const.NOT_FOUND_CODE)
                .WithMessage(Const.NOT_FOUND_MSG)
                .Build();

        if (!BCrypt.Net.BCrypt.Verify(query.Password, user.Password))
            return new ResponseBuilder()
                .WithStatus(Const.NOT_FOUND_CODE)
                .WithMessage("The password does not match.")
                .Build();

        var result = _mapper.Map<UserResult>(user);

        // Tạo cặp khóa RSA
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            try
            {
                // Lấy public key (dạng XML) để lưu vào database
                string publicKey = rsa.ToXmlString(false);

                // 🟢 Tạo kid từ publicKey (hash SHA256)
                string kid = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(publicKey)));

// Tạo access token với private key
                string accessToken = CreateToken(result, rsa, "AccessToken", kid);

// Tạo refresh token với kid giống access token
                string refreshTokenValue = CreateToken(result, rsa, "RefreshToken", kid);

// Lưu refresh token và public key vào database
                var refreshTokenCreateCommand = new RefreshTokenCreateCommand
                {
                    UserId = user.Id,
                    Token = refreshTokenValue,
                    PublicKey = publicKey,
                    KeyId = kid // 🟢 Lưu kid vào database
                };

                var res = _refreshTokenService.CreateOrUpdate<RefreshTokenResult>(refreshTokenCreateCommand).Result;

                if (res.Status != 1)
                    return new ResponseBuilder()
                        .WithStatus(Const.FAIL_CODE)
                        .WithMessage(Const.FAIL_SAVE_MSG)
                        .Build();

                var refreshToken = res.Data as RefreshTokenResult;

                if (refreshToken == null)
                    return new ResponseBuilder()
                        .WithStatus(Const.FAIL_CODE)
                        .WithMessage("Error while saving refresh token.")
                        .Build();

                SaveHttpOnlyCookie(accessToken, refreshToken.Token!);

                return new ResponseBuilder()
                    .WithStatus(Const.SUCCESS_CODE)
                    .WithMessage("Login successful.")
                    .Build();
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
        if (businessResult.Status != 1) return businessResult;

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

        if (businessResult.Status != 1) return (businessResult);

        #endregion

        #region CheckRefreshToken is valid => return user

        var tokenResult = businessResult.Data as TokenResult;
        businessResult = GetUserByToken(tokenResult.AccessToken).Result;

        return businessResult;

        #endregion
    }
   
    public async Task<BusinessResult> RefreshToken(UserRefreshTokenCommand request)
    {
        // Bước 1: Lấy thông tin refresh token từ database
        var refreshTokenEntity = await _refreshTokenRepository.GetByRefreshTokenAsync(request.RefreshToken);
        if (refreshTokenEntity == null)
        {
            return new ResponseBuilder()
                .WithStatus(Const.NOT_FOUND_CODE)
                .WithMessage("Refresh token not found.")
                .Build();
        }

        // Bước 2: Xác thực chữ ký JWT bằng public key
        var isValid = ValidateJwtSignature(request.RefreshToken, refreshTokenEntity.PublicKey);
        if (!isValid)
        {
            return new ResponseBuilder()
                .WithStatus(Const.FAIL_CODE)
                .WithMessage("Invalid refresh token signature.")
                .Build();
        }

        // Bước 3: Kiểm tra IP của request có khớp với IP đã lưu không
        var businessResult = _refreshTokenService.ValidateRefreshTokenIpMatch();

        if (businessResult.Status != 1)
        {
            return new ResponseBuilder()
                .WithStatus(Const.FAIL_CODE)
                .WithMessage("IP address mismatch.")
                .Build();
        }

        // Bước 4: Tạo cặp khóa RSA mới
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            try
            {
                // Lấy public key mới (dạng XML) để lưu vào database
                string newPublicKey = rsa.ToXmlString(false);

                // Bước 5: Tạo access token mới với private key mới
                var user = await _userRepository.GetById(refreshTokenEntity.UserId!.Value);
                var userResult = _mapper.Map<UserResult>(user);
                // 🟢 Tạo kid từ publicKey (hash SHA256)
                string kid = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(newPublicKey)));

// Tạo access token với private key
                string newAccessToken = CreateToken(userResult, rsa, "AccessToken", kid);

// Tạo refresh token với kid giống access token
                string newRefreshToken = CreateToken(userResult, rsa, "RefreshToken", kid);

                // Bước 7: Cập nhật refresh token trong database
                refreshTokenEntity.Token = newRefreshToken;
                refreshTokenEntity.PublicKey = newPublicKey;
                refreshTokenEntity.KeyId = kid;
                refreshTokenEntity.Expiry = DateTime.UtcNow.AddDays(_tokenSetting.RefreshTokenExpiryDays); 
                _refreshTokenRepository.Update(refreshTokenEntity);
                var isSaveChanges = await _unitOfWork.SaveChanges();
                if (!isSaveChanges)
                {
                    return new ResponseBuilder()
                        .WithStatus(Const.FAIL_CODE)
                        .WithMessage("Refresh token validation failed when saving changes.")
                        .Build();
                }
                
                // Bước 8: Lưu access token vào cookie (nếu cần)
                SaveHttpOnlyCookie(newAccessToken, newRefreshToken);

                // Bước 9: Trả về access token và refresh token mới
                var tokenResult = new TokenResult
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                };

                return new ResponseBuilder<TokenResult>()
                    .WithData(tokenResult)
                    .WithStatus(Const.SUCCESS_CODE)
                    .WithMessage("Token refreshed successfully.")
                    .Build();
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
                return new ResponseBuilder()
                    .WithStatus(Const.NOT_FOUND_CODE)
                    .WithMessage("You are not logged in, please log in to continue.")
                    .Build();
            _refreshTokenRepository.DeletePermanently(userRefreshToken);

            var isSaved = await _unitOfWork.SaveChanges();
            if (!isSaved) throw new Exception();

            _httpContextAccessor.HttpContext.Response.Cookies.Delete("accessToken");
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");

            return new ResponseBuilder()
                .WithStatus(Const.SUCCESS_CODE)
                .WithMessage("The account has been logged out.")
                .Build();
        }
        catch (Exception e)
        {
            return new ResponseBuilder()
                .WithStatus(Const.FAIL_CODE)
                .WithMessage(e.Message)
                .Build();
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
    //         Expires = DateTime.UtcNow.AddMinutes(_tokenSetting.AccessTokenExpiryMinutes),
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
        {
            return new ResponseBuilder()
                .WithStatus(Const.NOT_FOUND_CODE)
                .WithMessage("No access token provided")
                .Build();
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(accessToken);

        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        if (string.IsNullOrEmpty(userId))
            return new ResponseBuilder()
                .WithStatus(Const.NOT_FOUND_CODE)
                .WithMessage("Error the access token provided")
                .Build();

        var businessResult = await _userService.GetById<UserResult>(Guid.Parse(userId));

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
            return new ResponseBuilder()
                .WithStatus(Const.NOT_FOUND_CODE)
                .WithMessage("You are not logged in, please log in to continue.")
                .Build();

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
            {
                return new ResponseBuilder()
                    .WithStatus(Const.NOT_FOUND_CODE)
                    .WithMessage("No user found.")
                    .Build();
            }

            // Lấy thông tin người dùng từ database
            var userResult = await _userService.GetById<UserResult>(userId.Value);
            return userResult;
        }
        catch (Exception ex)
        {
            return new ResponseBuilder()
                .WithStatus(Const.FAIL_CODE)
                .WithMessage(ex.Message)
                .Build();
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
            new Claim("Id", user.Id.ToString()),
            new Claim("Role", user.Role.ToString()),
            new Claim("TokenType", tokenType) // 🟢 Thêm claim để phân biệt
        };

        var key = new RsaSecurityKey(rsa)
        {
            KeyId = kid
        };
        var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: tokenType == "AccessToken" 
                ? DateTime.Now.AddMinutes(_tokenSetting.AccessTokenExpiryMinutes) // Access token ngắn hạn
                : DateTime.Now.AddDays(_tokenSetting.RefreshTokenExpiryDays),    // Refresh token dài hạn
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
            Expires = DateTime.UtcNow.AddMinutes(_tokenSetting.AccessTokenExpiryMinutes),
        };

        var refreshTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(_tokenSetting.RefreshTokenExpiryDays)
        };

        httpContext.Response.Cookies.Append("accessToken", accessToken, accessTokenOptions);
        httpContext.Response.Cookies.Append("refreshToken", refreshToken, refreshTokenOptions);
    }
}