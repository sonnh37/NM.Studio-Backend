using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Queries.Users;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IUserService : IBaseService
{
    Task<BusinessResult> UpdatePassword(UserPasswordCommand userPasswordCommand);

    Task<BusinessResult> Create(UserCreateCommand createCommand);

    Task<BusinessResult> Update(UserUpdateCommand updateCommand);

    Task<BusinessResult> AddUser(UserCreateCommand user);

    Task<BusinessResult> GetByUsername(string username);

    BusinessResult SendEmail(string email);

    BusinessResult ValidateOtp(string email, string otpInput);

    // Task<BusinessResult> RegisterByGoogleAsync(UserCreateByGoogleTokenCommand request);
    //
    // Task<BusinessResult> LoginByGoogleTokenAsync(VerifyGoogleTokenRequest request);
    //
    // Task<BusinessResult> FindAccountRegisteredByGoogle(VerifyGoogleTokenRequest request);

    Task<BusinessResult> GetByUsernameOrEmail(string key);

    Task<BusinessResult> GetByRefreshToken(UserGetByRefreshTokenQuery request);
}