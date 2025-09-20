using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Commands.UserTokens;
using NM.Studio.Domain.CQRS.Queries.Users;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models;
using NM.Studio.Domain.Models.Options;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;

namespace NM.Studio.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly IMapper _mapper;
    private readonly IUserTokenRepository _userTokenRepository;
    private readonly IUserTokenService _userTokenService;
    protected readonly UserJwtOptions _jwtOptions;
    protected readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AuthService(IMapper mapper,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        IUserService userService,
        ITokenService tokenService,
        IOptions<UserJwtOptions> jwtOptions,
        IUserTokenService userTokenService
    )
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _httpContextAccessor ??= new HttpContextAccessor();
        _jwtOptions = jwtOptions.Value;
        _userRepository = _unitOfWork.UserRepository;
        _userService = userService;
        _userTokenService = userTokenService;
        _userTokenRepository = _unitOfWork.UserTokenRepository;
        _tokenService = tokenService;
    }

    public async Task<BusinessResult> Login(AuthQuery query)
    {
        var user = await _userRepository.FindUsernameOrEmail(query.Account);
        if (user == null)
            throw new NotFoundException(Const.NOT_FOUND_MSG);

        if (!BCrypt.Net.BCrypt.Verify(query.Password, user.Password))
            throw new DomainException("The password does not match.");

        var accessToken = _tokenService.GenerateJwtToken(user);
        var refreshTokenValue = _tokenService.GenerateRefreshToken();
        var refreshTokenCreateCommand = new UserTokenCreateCommand
        {
            UserId = user.Id,
            Token = refreshTokenValue,
            Expiry = DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryInDays)
        };
        var res = await _userTokenService.CreateOrUpdate(refreshTokenCreateCommand);
        if (res.Status != nameof(Status.OK))
            return res;

        var tokenPair = new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue
        };
        return new BusinessResult(tokenPair, "Login successfully.");
    }

    public async Task<BusinessResult> RefreshToken(UserRefreshTokenCommand request)
    {
        // From token expired
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        var userId = principal.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        if (userId == null)
        {
            throw new UnauthorizedException("Invalid token");
        }

        // Get user token from db
        var userToken = await _userTokenRepository.GetQueryable(x =>
                    x.RefreshToken != null && request.RefreshToken != null &&
                    x.RefreshToken.ToLower() == request.RefreshToken.ToLower(),
                false)
            .SingleOrDefaultAsync();
        if (userToken == null || userToken.RefreshToken != request.RefreshToken || userToken.ExpiryTime <= DateTimeOffset.UtcNow)
        {
            throw new UnauthorizedException("Invalid refresh token");
        }

        // Address
        // var businessResult = _userTokenService.ValidateRefreshTokenIpMatch();
        // if (businessResult.Status != nameof(Status.OK))
        //     return businessResult;

        // Modify userToken
        var user = await _userRepository.GetQueryable(m => m.Id == userToken.UserId)
            .SingleOrDefaultAsync();

        var newAccessToken = _tokenService.GenerateJwtToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        userToken.RefreshToken = newRefreshToken;
        userToken.ExpiryTime = DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryInDays);
        _userTokenRepository.Update(userToken);

        var isSaveChanges = await _unitOfWork.SaveChanges();
        if (!isSaveChanges)
            throw new Exception("Failed to update refresh token.");

        var tokenResult = new TokenResult
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };

        return new BusinessResult(tokenResult, "Token refreshed successfully.");
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

        return new BusinessResult
        {
            Message = "The account has been logged out."
        };
    }


    public Task<BusinessResult> RegisterByGoogleAsync(UserCreateByGoogleTokenCommand request)
    {
        throw new NotImplementedException();
    }

    public Task<BusinessResult> LoginByGoogleTokenAsync(VerifyGoogleTokenRequest request)
    {
        throw new NotImplementedException();
    }

    // protected async Task<BusinessResult> GetUserByToken(string accessToken)
    // {
    //     if (string.IsNullOrEmpty(accessToken))
    //         throw new DomainException("No access token provided");
    //
    //     var tokenHandler = new JwtSecurityTokenHandler();
    //     var jwtToken = tokenHandler.ReadJwtToken(accessToken);
    //
    //     var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
    //     if (string.IsNullOrEmpty(userId))
    //         throw new DomainException("Error the access token provided");
    //
    //     var businessResult = await _userService.GetById(new UserGetByIdQuery { Id = Guid.Parse(userId) });
    //
    //     return businessResult;
    // }

}