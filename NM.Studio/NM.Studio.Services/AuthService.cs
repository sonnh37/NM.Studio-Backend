using System.IdentityModel.Tokens.Jwt;
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
    private readonly int _expirationMinutes;
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
        _expirationMinutes = int.Parse(_configuration["TokenSetting:AccessTokenExpiryMinutes"] ?? "30");
        _tokenSetting = tokenSetting.Value;
        _userRepository = _unitOfWork.UserRepository;
        _userService = userService;
        _refreshTokenService = refreshTokenService;
        _refreshTokenRepository = _unitOfWork.RefreshTokenRepository;
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

        var accessToken = CreateToken(result);

        // save refreshToken
        var refreshTokenCreateCommand = new RefreshTokenCreateCommand
        {
            UserId = user.Id,
            Token = GenerateRefreshToken(),
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
                .WithMessage("Error while save refresh token.")
                .Build();

        SaveHttpOnlyCookie(accessToken, refreshToken.Token!);

        return new ResponseBuilder()
            .WithStatus(Const.SUCCESS_CODE)
            .WithMessage("Login successful.")
            .Build();
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
        var refreshTokenResult = businessResult.Data as RefreshTokenResult;
        var userId = refreshTokenResult.UserId.Value;
        
        businessResult = SaveRefreshToken(userId, refreshToken).Result;

        if (businessResult.Status != 1) return (businessResult);

        #endregion

        #region CheckRefreshToken is valid => return user

        var tokenResult = businessResult.Data as TokenResult;
        businessResult = GetUserByToken(tokenResult.AccessToken).Result;

        return businessResult;

        #endregion
    }

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

    public async Task<BusinessResult> RefreshToken(UserRefreshTokenCommand request)
    {
        var businessResult = ValidateRefreshTokenIpAdMatch(request.RefreshToken);
        if (businessResult.Status != 1) return businessResult;

        var refreshTokenResult = businessResult.Data as RefreshTokenResult;
        var userId = refreshTokenResult.UserId.Value;
        
        var res = await SaveRefreshToken(userId, request.RefreshToken);
        return res;
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

    private string CreateToken(UserResult user)
    {
        var claims = new List<Claim>
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("Role", user.Role.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(_expirationMinutes),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }

        return Convert.ToBase64String(randomNumber);
    }

    private async Task<BusinessResult> SaveRefreshToken(Guid userId, string refreshToken)
    {
        var user = await _userRepository.GetById(userId);
        var userResult = _mapper.Map<UserResult>(user);

        // Create new access token
        var accessToken = CreateToken(userResult);
        
        var accessTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(_tokenSetting.AccessTokenExpiryMinutes),
        };

        _httpContextAccessor.HttpContext.Response.Cookies.Append("accessToken", accessToken, accessTokenOptions);

        var tokenResult = new TokenResult { AccessToken = accessToken, RefreshToken = refreshToken };
        return new ResponseBuilder<TokenResult>()
            .WithData(tokenResult)
            .WithStatus(Const.SUCCESS_CODE)
            .WithMessage(Const.SUCCESS_LOGIN_MSG)
            .Build();
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
}