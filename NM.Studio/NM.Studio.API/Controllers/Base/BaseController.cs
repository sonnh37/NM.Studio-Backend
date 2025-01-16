using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Queries.Users;
using NM.Studio.Domain.Models;
using NM.Studio.Domain.Models.Responses;
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

    protected Task<BusinessResult> GetUserByCookie()
    {
        var request = new UserGetByCookieQuery();
        var businessResult = _mediator.Send(request);

        return businessResult;
    }
    
    protected async Task<BusinessResult> GetUserByToken(string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            return new ResponseBuilder()
                .WithStatus(Const.NOT_FOUND_CODE)
                .WithMessage("No access token provided")
                .Build();
        }
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(accessToken);

        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        
        var userGetByIdQuery = new UserGetByIdQuery
        {
            Id = Guid.Parse(userId)
        };
        var businessResult = await _mediator.Send(userGetByIdQuery);
        
        return businessResult;
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
        var businessResult = await _mediator.Send(request);
        if (businessResult.Status != 1) return businessResult;
        var _object = businessResult.Data as TokenResult;
        var accessTokenOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // Set true khi chạy trên HTTPS
            SameSite = SameSiteMode.None, // Đảm bảo chỉ gửi cookie trong cùng domain
            Expires = DateTime.UtcNow.AddMinutes(_tokenSetting.AccessTokenExpiryMinutes), // AccessToken có thể hết hạn sau 1 giờ
        };

        Response.Cookies.Append("accessToken", _object.Token, accessTokenOptions);
        return businessResult;
    }
}