using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Queries.Auths;
using NM.Studio.Domain.CQRS.Queries.Users;
using NM.Studio.Domain.Models;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;

namespace NM.Studio.API.Controllers;

[Route("auth")]
public class AuthController : BaseController
{
    public AuthController(IMediator mediator, IOptions<TokenSetting> tokenSetting) : base(mediator, tokenSetting)
    {
    }

    [AllowAnonymous]
    [HttpGet("info")]
    public IActionResult GetUserInfo([FromQuery] AuthGetByCookieQuery request)
    {
        var businessResult = _mediator.Send(request).Result;

        return Ok(businessResult);
    }

     [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthQuery authQuery)
    {
        var businessResult = await _mediator.Send(authQuery);
        if (businessResult.Status != 1) return Ok(businessResult);
        var _object = businessResult.Data as TokenResult;

        var accessTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // Set true khi chạy trên HTTPS
            SameSite = SameSiteMode.None, // Đảm bảo chỉ gửi cookie trong cùng domain
            Expires = DateTime.UtcNow.AddMinutes(_tokenSetting.AccessTokenExpiryMinutes),
        };

        var refreshTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // Set true khi chạy trên HTTPS
            SameSite = SameSiteMode.None, // Đảm bảo chỉ gửi cookie trong cùng domain
            Expires = DateTime.UtcNow.AddDays(_tokenSetting.RefreshTokenExpiryDays)
        };


        // Set cookies vào HttpContext
        Response.Cookies.Append("accessToken", _object.Token, accessTokenOptions);
        Response.Cookies.Append("refreshToken", _object.RefreshToken, refreshTokenOptions);

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
        var businessResult = await _mediator.Send(userLogoutCommand);

        if (businessResult.Status != 1) return Ok(businessResult);

        Response.Cookies.Delete("accessToken");
        Response.Cookies.Delete("refreshToken");

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] UserRefreshTokenCommand request)
    {
        var msg = await base.RefreshToken();
        return Ok(msg);
    }

    [AllowAnonymous]
    // POST api/<AuthController>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserCreateCommand request)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        request.Password = passwordHash;

        var businessResult = await _mediator.Send(request);
        return Ok(businessResult);
    }
    
    [AllowAnonymous]
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPQuery request)
    {
        var businessResult = await _mediator.Send(request);
        return Ok(businessResult);
    }
    
    [AllowAnonymous]
    [HttpPost("login-by-google")]
    public async Task<IActionResult> LoginByGoogle([FromBody] AuthByGoogleTokenQuery request)
    {
        var businessResult = await _mediator.Send(request);
        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpPost("register-by-google")]
    public async Task<IActionResult> RegisterByGoogle([FromBody] UserCreateByGoogleTokenCommand request)
    {
        var businessResult = await _mediator.Send(request);
        return Ok(businessResult);
    }
    
}