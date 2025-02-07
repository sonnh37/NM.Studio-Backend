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
    public UserController(IMediator mediator) : base(mediator)
    {
    }

    [Authorize(Roles = "Admin")]
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

    // [Authorize(Roles = "Admin,Staff")]
    // [HttpGet("account")]
    // [AllowAnonymous]
    // public async Task<IActionResult> GetAccount([FromQuery] UserGetByAccountQuery request)
    // {
    //     var msg = await _mediator.Send(request);
    //     return Ok(msg);
    // }
    //
    // [AllowAnonymous]
    // [HttpPost("send-email")]
    // public async Task<IActionResult> SendOTP([FromBody] UserSendEmailQuery request)
    // {
    //     var businessResult = await _mediator.Send(request);
    //     return Ok(businessResult);
    // }
    //
    // [AllowAnonymous]
    // [HttpPost("find-account-registered-by-google")]
    // public async Task<IActionResult> FindAccountRegisteredByGoogle([FromBody] UserGetByGoogleTokenQuery request)
    // {
    //     var businessResult = await _mediator.Send(request);
    //     return Ok(businessResult);
    // }
}