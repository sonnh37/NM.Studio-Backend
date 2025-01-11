using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Albums;
using NM.Studio.Domain.Enums;
using NM.Studio.Domain.Models;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;

namespace NM.Studio.API.Controllers;

[Route("users")]
public class UserController : BaseController
{
    public UserController(IMediator mediator, IOptions<TokenSetting> tokenSetting) : base(mediator, tokenSetting)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] UserGetAllQuery userGetAllQuery)
    {
        var businessResult = await _mediator.Send(userGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var userGetByIdQuery = new UserGetByIdQuery
        {
            Id = id
        };
        var businessResult = await _mediator.Send(userGetByIdQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("info")]
    public async Task<IActionResult> GetUserForLimitRoles()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (refreshToken == null)
        {
            // refreshToken is unvalid
            return Ok(new ResponseBuilder()
                .WithStatus(Const.NOT_FOUND_CODE)
                .WithMessage("Pls, login again.")
                .Build());
        }

        // check refreshToken is valid
        var message = IsLoggedIn(refreshToken).Result;
        if (message.Status != 1)
        {
            return Ok(message);
        }

        // check AccessToken is valid
        var accessToken = Request.Cookies["accessToken"];
        if (accessToken != null)
        {
            // accessToken và refreshToken is available
            var msg = await GetCurrentUser();

            return Ok(msg);
        }

        var userRefresh = new UserRefreshTokenCommand
        {
            // accessToken is unvalid
            RefreshToken = refreshToken
        };
        var message_ = await _mediator.Send(userRefresh);
        if (message_.Status != 1) return Ok(message_);
        var _object = message_.Data as TokenResult;
        var accessTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(_tokenSetting.AccessTokenExpiryMinutes),
        };

        Response.Cookies.Append("accessToken", _object.Token, accessTokenOptions);

        // Get info user 
        if (string.IsNullOrEmpty(_object.Token))
        {
            return Ok("Access token is error");
        }

        // accessToken và refreshToken is available
        var businessResult = await GetCurrentUser(_object.Token);

        return Ok(businessResult);
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateCommand userCreateCommand)
    {
        var businessResult = await _mediator.Send(userCreateCommand);

        return Ok(businessResult);
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserUpdateCommand userUpdateCommand)
    {
        var businessResult = await _mediator.Send(userUpdateCommand);

        return Ok(businessResult);
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpPut("restore")]
    public async Task<IActionResult> UpdateIsDeleted([FromBody] UserRestoreCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpPut("password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UserPasswordCommand userUpdateCommand)
    {
        var businessResult = await _mediator.Send(userUpdateCommand);

        return Ok(businessResult);
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] UserDeleteCommand userDeleteCommand)
    {
        var businessResult = await _mediator.Send(userDeleteCommand);

        return Ok(businessResult);
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpGet("account")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAccount([FromQuery] UserGetByAccountQuery request)
    {
        var msg = await _mediator.Send(request);
        return Ok(msg);
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

    // [HttpGet("get-token")]
    // public Task<IActionResult> GetToken()
    // {
    //     try
    //     {
    //         // because [authorize] => accessToken chắc chắn có nếu ko tự động trả veef 401 và refresh-token hàm ở trên
    //         var accessToken = Request.Cookies["accessToken"];
    //         var refreshToken = Request.Cookies["refreshToken"];
    //         var userRefreshToken = new UserGetByRefreshTokenQuery
    //         {
    //             RefreshToken = refreshToken
    //         };
    //         var message = _mediator.Send(userRefreshToken).Result;
    //         if(message.Status != 1) return Ok(message);
    //
    //         var res = new TokenResponse { AccessToken = accessToken, RefreshToken = null };
    //         return Ok(ResponseHelper.Success<TokenResponse>(res));
    //     }
    //     catch (Exception ex)
    //     {
    //         return Ok(ResponseHelper.Error("Error system."));
    //     }
    // }

    [AllowAnonymous]
    [HttpGet("is-logged-in")]
    public async Task<IActionResult> IsLogged()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (refreshToken == null)
        {
            return Unauthorized();
        }

        // check refreshToken is exist db
        var message = await IsLoggedIn(refreshToken);
        return Ok(message);
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

    [HttpPost("decode-token")]
    public async Task<IActionResult> DecodeToken([FromBody] DecodedTokenQuery request)
    {
        var businessResult = await _mediator.Send(request);
        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpPost("send-email")]
    public async Task<IActionResult> SendOTP([FromBody] UserSendEmailQuery request)
    {
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
    [HttpPost("find-account-registered-by-google")]
    public async Task<IActionResult> FindAccountRegisteredByGoogle([FromBody] UserGetByGoogleTokenQuery request)
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