using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.Models.Responses;
using MediatR;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Handler.Commands.Base;

namespace NM.Studio.Handler.Commands;

public class UserCommandHandler : BaseCommandHandler,
    IRequestHandler<UserUpdateCommand, BusinessResult>,
    IRequestHandler<UserDeleteCommand, BusinessResult>,
    IRequestHandler<UserCreateCommand, BusinessResult>,
    IRequestHandler<UserCreateByGoogleTokenCommand, BusinessResult>,
    IRequestHandler<UserRefreshTokenCommand, BusinessResult>
{
    private readonly IUserService _userService;

    public UserCommandHandler(IUserService userService) : base(userService)
    {
        _userService = userService;
    }

    public async Task<BusinessResult> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _userService.CreateOrUpdate<UserResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _baseService.DeleteById(request.Id, request.IsPermanent);
        return msgView;
    }

    public async Task<BusinessResult> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _baseService.CreateOrUpdate<UserResult>(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(UserCreateByGoogleTokenCommand request, CancellationToken cancellationToken)
    {
        return await _userService.RegisterByGoogleAsync(request);
    }

    public async Task<BusinessResult> Handle(UserRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await _userService.RefreshToken(request);
    }
}