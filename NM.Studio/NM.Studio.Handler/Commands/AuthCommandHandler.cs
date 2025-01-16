using Google.Apis.Auth.OAuth2.Web;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.Responses;
using MediatR;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Handler.Commands.Base;

namespace NM.Studio.Handler.Commands;

public class AuthCommandHandler :
    IRequestHandler<UserLogoutCommand, BusinessResult>,
    IRequestHandler<UserRefreshTokenCommand, BusinessResult>
{
    protected readonly IAuthService _authService;

    public AuthCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<BusinessResult> Handle(UserLogoutCommand request, CancellationToken cancellationToken)
    {
        return await _authService.Logout(request);
    }

    public async Task<BusinessResult> Handle(UserRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await _authService.RefreshToken(request);
    }
}