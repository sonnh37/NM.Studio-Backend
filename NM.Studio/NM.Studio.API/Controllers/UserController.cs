﻿using NM.Studio.Domain.CQRS.Commands.Users;
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
    public IActionResult GetUserInfo()
    {
        #region CheckAuthen

        var refreshToken = Request.Cookies["refreshToken"];
        if (refreshToken == null)
        {
            // refreshToken is unvalid
            return Ok(new ResponseBuilder()
                .WithStatus(Const.NOT_FOUND_CODE)
                .WithMessage("Pls, login again.")
                .Build());
        }

        // check in db have refreshToken 
        var message = IsLoggedIn(refreshToken).Result;
        if (message.Status != 1)
        {
            return Ok(message);
        }
        
        #endregion
        
        // check AccessToken from client is valid
        var accessToken = Request.Cookies["accessToken"];
        if (accessToken != null)
        {
            var msg = GetUserByCookie().Result;

            return Ok(msg);
        }

        var userRefresh = new UserRefreshTokenCommand
        {
            RefreshToken = refreshToken
        };
        var message_ = _mediator.Send(userRefresh).Result;
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

        var businessResult = GetUserByToken(_object.Token).Result;

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

    // [HttpPost("decode-token")]
    // public async Task<IActionResult> DecodeToken([FromBody] DecodedTokenQuery request)
    // {
    //     var businessResult = await _mediator.Send(request);
    //     return Ok(businessResult);
    // }

    [AllowAnonymous]
    [HttpPost("send-email")]
    public async Task<IActionResult> SendOTP([FromBody] UserSendEmailQuery request)
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
}