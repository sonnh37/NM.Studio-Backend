using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;
using MediatR;
using NM.Studio.Domain.CQRS.Queries.Auths;
using NM.Studio.Domain.CQRS.Queries.Users;

namespace NM.Studio.Handler.Queries;

public class AuthQueryHandler :
    IRequestHandler<AuthQuery, BusinessResult>,
    IRequestHandler<AuthGetByCookieQuery, BusinessResult>
{
    protected readonly IAuthService _authService;

    public AuthQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<BusinessResult> Handle(AuthQuery request, CancellationToken cancellationToken)
    {
        return await _authService.Login(request);
    }

    public Task<BusinessResult> Handle(AuthGetByCookieQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_authService.GetUserByCookie(request));
    }
}