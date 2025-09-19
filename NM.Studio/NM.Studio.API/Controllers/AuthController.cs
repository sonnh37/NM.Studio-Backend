using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Queries.Auths;
using NM.Studio.Domain.CQRS.Queries.Users;

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
    [HttpGet("info")]
    public IActionResult GetUserInfo([FromQuery] AuthGetByCookieQuery request)
    {
        var businessResult = _authService.GetUserByCookie(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthQuery authQuery)
    {
        var businessResult = await _authService.Login(authQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var userLogoutCommand = new UserLogoutCommand
        {
            RefreshToken = refreshToken
        };
        var businessResult = await _authService.Logout(userLogoutCommand);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var request = new UserRefreshTokenCommand
        {
            RefreshToken = refreshToken
        };
        var businessResult = await _authService.RefreshToken(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    // POST api/<AuthController>
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