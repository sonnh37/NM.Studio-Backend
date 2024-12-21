using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.Domain.CQRS.Queries.Users;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results;

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
        var userId = Response.HttpContext.User.FindFirst("Id")?.Value;
        
        
        if (string.IsNullOrEmpty(userId)) return null;
        
        var userGetByIdQuery = new UserGetByIdQuery
        {
            Id = Guid.Parse(userId)
        };
        var messageResult = await _mediator.Send(userGetByIdQuery);

        return messageResult;
    }
}