using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Queries.Users;

namespace NM.Studio.API.Controllers;

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
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] UserGetByIdQuery request)
    {
        var businessResult = await _mediator.Send(request);

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
    [HttpPut("update-cache")]
    public async Task<IActionResult> UpdateUserCache([FromBody] UserUpdateCacheCommand request)
    {
        var businessResult = await _mediator.Send(request);

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