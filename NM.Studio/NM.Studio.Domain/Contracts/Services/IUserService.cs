using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Queries.Users;
using NM.Studio.Domain.Models;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Domain.Contracts.Services;

public interface IUserService : IBaseService
{
    Task<BusinessResult> UpdatePassword(UserPasswordCommand userPasswordCommand);

    Task<BusinessResult> Create(UserCreateCommand createCommand);

    Task<BusinessResult> Update(UserUpdateCommand updateCommand);
    
    Task<BusinessResult> Login(AuthQuery authQuery);

    Task<BusinessResult> AddUser(UserCreateCommand user);

    Task<BusinessResult> GetByUsername(string username);

    BusinessResult DecodeToken(string token);

    BusinessResult SendEmail(string email);

    BusinessResult ValidateOtp(string email, string otpInput);
    
    Task<BusinessResult> RegisterByGoogleAsync(UserCreateByGoogleTokenCommand request);
    
    Task<BusinessResult> LoginByGoogleTokenAsync(VerifyGoogleTokenRequest request);
    
    Task<BusinessResult> FindAccountRegisteredByGoogle(VerifyGoogleTokenRequest request);
    
    Task<BusinessResult> GetByUsernameOrEmail(string key);
    
    Task<BusinessResult> RefreshToken(UserRefreshTokenCommand request);

    Task<BusinessResult> Logout(UserLogoutCommand userLogoutCommand);
}