using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.Domain.CQRS.Queries.Users;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Utilities;

namespace NM.Studio.API.Controllers.Base;

[ApiController]
public class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;

    protected BaseController()
    {
    }

    protected BaseController(IMediator mediator) : this()
    {
        _mediator = mediator;
    }

    protected async Task<BusinessResult> GetCurrentUser()
    {
        var message = IsLoggedIn().Result;
        if(message.Status != 1) return message;
        
        var userId = Response.HttpContext.User.FindFirst("Id")?.Value;
        
        var userGetByIdQuery = new UserGetByIdQuery
        {
            Id = userId != null ? Guid.Parse(userId) : Guid.Empty
        };
        var messageResult = await _mediator.Send(userGetByIdQuery);

        return messageResult;
    }
    
    protected async Task<BusinessResult> IsLoggedIn()
    {
        // check refreshToken is exist db
        var refreshToken = Request.Cookies["refreshToken"];
        var userRefreshToken = new UserGetByRefreshTokenQuery
        {
            RefreshToken = refreshToken
        };
        var message = await _mediator.Send(userRefreshToken);

        return message;
    } 
}