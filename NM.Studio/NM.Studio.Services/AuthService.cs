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
    private readonly IUserRefreshTokenRepository _userRefreshTokenRepository;
    private readonly int _expirationMinutes;
    private readonly string _clientId;
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    protected readonly TokenSetting _tokenSetting;

    public AuthService(IMapper mapper,
        IUnitOfWork unitOfWork, 
        IConfiguration configuration,
        IUserService userService,
        IOptions<TokenSetting> tokenSetting
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
        _userRefreshTokenRepository = _unitOfWork.UserRefreshTokenRepository;
        
    }

    public async Task<BusinessResult> Login(AuthQuery query)
    {
        var user = await _userRepository.FindUsernameOrEmail(query.Account);
        var result = _mapper.Map<UserResult>(user);
        //check username 
        if (user == null)
            return new ResponseBuilder()
                .WithStatus(Const.NOT_FOUND_CODE)
                .WithMessage("The account does not exist.")
                .Build();

        //check password
        if (!BCrypt.Net.BCrypt.Verify(query.Password, user.Password))
            return new ResponseBuilder()
                .WithStatus(Const.NOT_FOUND_CODE)
                .WithMessage("The password does not match.")
                .Build();

        var (token, expiration) = CreateToken(result);

        var refreshToken = GenerateRefreshToken();

        await SaveRefreshToken(user.Id, refreshToken);

        var tokenResult = new TokenResult { Token = token, RefreshToken = refreshToken };
        return new ResponseBuilder<TokenResult>()
            .WithData(tokenResult)
            .WithStatus(Const.SUCCESS_CODE)
            .WithMessage(Const.SUCCESS_LOGIN_MSG)
            .Build();
    }

    public BusinessResult GetUserByCookie(AuthGetByCookieQuery request)
    {
        #region CheckAuthen

        var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
        if (refreshToken == null)
        {
            // refreshToken is unvalid
            return (new ResponseBuilder()
                .WithStatus(Const.NOT_FOUND_CODE)
                .WithMessage("Pls, login again.")
                .Build());
        }

        // check in db have refreshToken 
        var message = _userService.GetByRefreshToken(new UserGetByRefreshTokenQuery{ RefreshToken = refreshToken }).Result;
        if (message.Status != 1)
        {
            return (message);
        }
        
        #endregion
        
        // check AccessToken from client is valid
        var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["accessToken"];
        if (accessToken != null)
        {
            var br = GetUserByCookie();

            return (br);
        }

        var userRefresh = new UserRefreshTokenCommand
        {
            RefreshToken = refreshToken
        };
        var message_ = this.RefreshToken(userRefresh).Result;
        if (message_.Status != 1) return (message_);
        var _object = message_.Data as TokenResult;
        var accessTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(_tokenSetting.AccessTokenExpiryMinutes),
        };

        _httpContextAccessor.HttpContext.Response.Cookies.Append("accessToken", _object.Token, accessTokenOptions);

        var businessResult = GetUserByToken(_object.Token).Result;

        return (businessResult);
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
        if (string.IsNullOrEmpty(userId)) return new ResponseBuilder()
            .WithStatus(Const.NOT_FOUND_CODE)
            .WithMessage("Error the access token provided")
            .Build();
        
        var businessResult = await _userService.GetById<UserResult>(Guid.Parse(userId));
        
        return businessResult;
    }

    public async Task<BusinessResult> RefreshToken(UserRefreshTokenCommand request)
    {
        if (request.RefreshToken == null)
            return new ResponseBuilder()
                    .WithStatus(Const.NOT_FOUND_CODE)
                    .WithMessage("You are not logged in, please log in to continue.")
                    .Build();
        var refreshToken = request.RefreshToken;

        // Validate refresh token from request
        var storedRefreshToken = await _userRefreshTokenRepository.GetByRefreshTokenAsync(refreshToken);

        if (storedRefreshToken == null || storedRefreshToken.ExpirationDate < DateTime.UtcNow)
        {
            return new ResponseBuilder()
                .WithStatus(Const.FAIL_CODE)
                .WithMessage("Your session has expired. Please log in again.")
                .Build();
        }

        // Get user info from the refresh token
        var user = await _userRepository.GetById(storedRefreshToken.UserId);
        var userResult = _mapper.Map<UserResult>(user);

        // Create new access token
        var (token, expiration) = CreateToken(userResult);

        var tokenResult = new TokenResult { Token = token, RefreshToken = refreshToken };
        return new ResponseBuilder<TokenResult>()
            .WithData(tokenResult)
            .WithStatus(Const.SUCCESS_CODE)
            .WithMessage(Const.SUCCESS_LOGIN_MSG)
            .Build();
    }
    
    public async Task<BusinessResult> Logout(UserLogoutCommand userLogoutCommand)
    {
        try
        {
            var userRefreshToken = await _userRefreshTokenRepository
                .GetByRefreshTokenAsync(userLogoutCommand.RefreshToken ?? string.Empty);
            if (userRefreshToken == null)
                return new ResponseBuilder()
                    .WithStatus(Const.NOT_FOUND_CODE)
                    .WithMessage("You are not logged in, please log in to continue.")
                    .Build();
            _userRefreshTokenRepository.DeletePermanently(userRefreshToken);

            var isSaved = await _unitOfWork.SaveChanges();
            if (!isSaved) throw new Exception();

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

    private (string token, string expiration) CreateToken(UserResult user)
    {
        var claims = new List<Claim>
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("Role", user.Role.ToString()),
            new Claim("Expiration",
                new DateTimeOffset(DateTime.Now.AddMinutes(_expirationMinutes)).ToUnixTimeSeconds().ToString())
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

        return (jwt, DateTime.Now.AddMinutes(_expirationMinutes).ToString("o"));
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

    private async Task SaveRefreshToken(Guid userId, string refreshToken)
    {
        var expirationDate = DateTime.UtcNow.AddMonths(1); // Refresh token expires in 1 month

        var refreshTokenEntity = new UserRefreshToken
        {
            UserId = userId,
            RefreshToken = refreshToken,
            ExpirationDate = expirationDate
        };
        _userRefreshTokenRepository.Add(refreshTokenEntity);
        await _unitOfWork.SaveChanges();
    }
    
    public BusinessResult GetUserByCookie()
    {
        try
        {
            if (_httpContextAccessor?.HttpContext == null ||
                !_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                return new ResponseBuilder()
                    .WithStatus(Const.NOT_FOUND_CODE)
                    .WithMessage("Not login yet.")
                    .Build();

            // Lấy thông tin UserId từ Claims
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("Id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return new ResponseBuilder()
                    .WithStatus(Const.NOT_FOUND_CODE)
                    .WithMessage("No user found.")
                    .Build();

            // Lấy thêm thông tin User từ database nếu cần
            var userId = Guid.Parse(userIdClaim);
            var user = _unitOfWork.UserRepository.GetById(userId).Result;
            var userResult = _mapper.Map<UserResult>(user);

            if (userResult == null) return new ResponseBuilder()
                .WithStatus(Const.NOT_FOUND_CODE)
                .WithMessage("Not found.")
                .Build();
            
            return new ResponseBuilder<UserResult>()
                .WithData(userResult)
                .WithStatus(Const.SUCCESS_CODE)
                .WithMessage(Const.SUCCESS_READ_MSG)
                .Build();
        }
        catch (Exception ex)
        {
            return new ResponseBuilder()
                .WithStatus(Const.FAIL_CODE)
                .WithMessage(ex.Message)
                .Build();
        }
    }
}