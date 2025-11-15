using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.CQRS.Commands.Users;
using NM.Studio.Domain.Models.CQRS.Queries.Users;

namespace NM.Studio.API.Controllers;

[Route("api/auth")]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthQuery authQuery)
    {
        var businessResult = await _authService.Login(authQuery);

        return Ok(businessResult);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] UserLogoutCommand request)
    {
        var businessResult = await _authService.Logout(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] UserRefreshTokenCommand request)
    {
        var businessResult = await _authService.RefreshToken(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserCreateCommand request)
    {
        var businessResult = await _userService.CreateOrUpdate(request);
        return Ok(businessResult);
    }

    // [AllowAnonymous]
    // [HttpPost("verify-otp")]
    // public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPQuery request)
    // {
    //     var businessResult = await _authService.Send(request);
    //     return Ok(businessResult);
    // }
    //
    // [AllowAnonymous]
    // [HttpPost("login-by-google")]
    // public async Task<IActionResult> LoginByGoogle([FromBody] AuthByGoogleTokenQuery request)
    // {
    //     var businessResult = await _authService.Send(request);
    //     return Ok(businessResult);
    // }
    //
    // [AllowAnonymous]
    // [HttpPost("register-by-google")]
    // public async Task<IActionResult> RegisterByGoogle([FromBody] UserCreateByGoogleTokenCommand request)
    // {
    //     var businessResult = await _authService.Send(request);
    //     return Ok(businessResult);
    // }
}