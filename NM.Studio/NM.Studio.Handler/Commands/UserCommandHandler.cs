using MediatR;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Handler.Commands;

public class UserCommandHandler :
    IRequestHandler<UserUpdateCommand, BusinessResult>,
    IRequestHandler<UserDeleteCommand, BusinessResult>,
    IRequestHandler<UserCreateCommand, BusinessResult>,
    // IRequestHandler<UserCreateByGoogleTokenCommand, BusinessResult>,
    IRequestHandler<UserPasswordCommand, BusinessResult>
{
    private readonly IUserService _userService;

    public UserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<BusinessResult> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _userService.Create(request);
        return msgView;
    }

    public async Task<BusinessResult> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _userService.Delete(request);
        return msgView;
    }

    // public async Task<BusinessResult> Handle(UserCreateByGoogleTokenCommand request, CancellationToken cancellationToken)
    // {
    //     return await _userService.RegisterByGoogleAsync(request);
    // }

    public async Task<BusinessResult> Handle(UserPasswordCommand request, CancellationToken cancellationToken)
    {
        return await _userService.UpdatePassword(request);
    }

    public async Task<BusinessResult> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
    {
        var msgView = await _userService.Update(request);
        return msgView;
    }
}