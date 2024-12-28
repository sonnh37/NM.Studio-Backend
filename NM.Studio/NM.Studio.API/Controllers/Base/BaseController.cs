using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Queries.Users;
using NM.Studio.Domain.Models;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;

namespace NM.Studio.API.Controllers.Base;

[ApiController]
public class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;
    protected readonly TokenSetting _tokenSetting;


    protected BaseController()
    {
    }

    protected BaseController(IMediator mediator) : this()
    {
        _mediator = mediator;
    }
    
    protected BaseController(IMediator mediator, IOptions<TokenSetting> tokenSetting) : this()
    {
        _mediator = mediator;
        _tokenSetting = tokenSetting.Value;
    }

    protected async Task<BusinessResult> GetCurrentUser()
    {
        var userId = Response.HttpContext.User.FindFirst("Id")?.Value;

        var userGetByIdQuery = new UserGetByIdQuery
        {
            Id = userId != null ? Guid.Parse(userId) : Guid.Empty
        };
        var messageResult = await _mediator.Send(userGetByIdQuery);

        return messageResult;
    }
    
    protected async Task<BusinessResult> GetCurrentUser(string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            return ResponseHelper.NotFound("Token not found");
        }
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(accessToken);

        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return ResponseHelper.NotFound("UserId not found");
        }
        
        var userGetByIdQuery = new UserGetByIdQuery
        {
            Id = Guid.Parse(userId)
        };
        var messageResult = await _mediator.Send(userGetByIdQuery);
        return messageResult;
    }

    protected async Task<BusinessResult> IsLoggedIn(string refreshToken)
    {
        var userRefreshToken = new UserGetByRefreshTokenQuery
        {
            RefreshToken = refreshToken
        };
        var message = await _mediator.Send(userRefreshToken);

        return message;
    }

    protected async Task<BusinessResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var request = new UserRefreshTokenCommand
        {
            RefreshToken = refreshToken
        };
        var messageResult = await _mediator.Send(request);
        if (messageResult.Status != 1) return messageResult;
        var _object = messageResult.Data as TokenResult;
        var accessTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // Set true khi chạy trên HTTPS
            SameSite = SameSiteMode.None, // Đảm bảo chỉ gửi cookie trong cùng domain
            Expires = DateTime.UtcNow.AddMinutes(_tokenSetting.AccessTokenExpiryMinutes), // AccessToken có thể hết hạn sau 1 giờ
        };

        Response.Cookies.Append("accessToken", _object.Token, accessTokenOptions);
        return ResponseHelper.GetToken(_object.Token, "");
    }
}