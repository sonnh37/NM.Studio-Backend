using System.Security.Cryptography;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Queries.Auths;
using NM.Studio.Domain.CQRS.Queries.Users;
using NM.Studio.Domain.Models;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IAuthService
{
    Task<BusinessResult> Login(AuthQuery authQuery);

    // BusinessResult GetUserByCookie(AuthGetByCookieQuery request);

    Task<BusinessResult> RefreshToken(UserRefreshTokenCommand request);

    Task<BusinessResult> Logout(UserLogoutCommand userLogoutCommand);

    Task<BusinessResult> RegisterByGoogleAsync(UserCreateByGoogleTokenCommand request);

    Task<BusinessResult> LoginByGoogleTokenAsync(VerifyGoogleTokenRequest request);
}