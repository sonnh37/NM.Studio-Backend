using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Queries.Users;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;
using MediatR;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.Models;

namespace NM.Studio.Handler.Queries;

public class UserQueryHandler :
    IRequestHandler<UserGetAllQuery, BusinessResult>,
    IRequestHandler<UserGetByIdQuery, BusinessResult>,
    IRequestHandler<UserGetByAccountQuery, BusinessResult>,
    IRequestHandler<AuthQuery, BusinessResult>,
    IRequestHandler<DecodedTokenQuery, BusinessResult>,
    IRequestHandler<UserSendEmailQuery, BusinessResult>,
    IRequestHandler<VerifyOTPQuery, BusinessResult>,
    IRequestHandler<UserGetByGoogleTokenQuery, BusinessResult>,
    IRequestHandler<AuthByGoogleTokenQuery, BusinessResult>,
    IRequestHandler<UserGetByRefreshTokenQuery, BusinessResult>,
    IRequestHandler<UserGetByCookieQuery, BusinessResult>
{
    protected readonly IUserService _userService;

    public UserQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<BusinessResult> Handle(UserGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _userService.GetAll<UserResult>(request);
    }

    public async Task<BusinessResult> Handle(UserGetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetById<UserResult>(request.Id);
    }

    public async Task<BusinessResult> Handle(AuthQuery request, CancellationToken cancellationToken)
    {
        return await _userService.Login(request);
    }

    public async Task<BusinessResult> Handle(UserGetByAccountQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetByUsernameOrEmail(request.account!);
    }

    public Task<BusinessResult> Handle(DecodedTokenQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_userService.DecodeToken(request.Token!));
    }

    public Task<BusinessResult> Handle(UserSendEmailQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_userService.SendEmail(request.Email!));
    }

    public Task<BusinessResult> Handle(VerifyOTPQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_userService.ValidateOtp(request.Email!, request.Otp!));
    }

    public async Task<BusinessResult> Handle(UserGetByGoogleTokenQuery request, CancellationToken cancellationToken)
    {
        return await _userService.FindAccountRegisteredByGoogle(new VerifyGoogleTokenRequest{ Token = request.Token!});
    }

    public async Task<BusinessResult> Handle(AuthByGoogleTokenQuery request, CancellationToken cancellationToken)
    {
        return await _userService.LoginByGoogleTokenAsync(new VerifyGoogleTokenRequest{ Token = request.Token!});
    }


    public async Task<BusinessResult> Handle(UserGetByRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetByRefreshToken(request);
    }

    public Task<BusinessResult> Handle(UserGetByCookieQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_userService.GetUserByCookie());
    }
}