using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Carts;
using NM.Studio.Domain.CQRS.Queries.Carts;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class CartController : BaseController
{
    public CartController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] CartGetAllQuery cartGetAllQuery)
    {
        var businessResult = await _mediator.Send(cartGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] CartGetByIdQuery request)
    {
        var businessResult = await _mediator.Send(request);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CartCreateCommand cartCreateCommand)
    {
        var businessResult = await _mediator.Send(cartCreateCommand);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CartUpdateCommand cartUpdateCommand)
    {
        var businessResult = await _mediator.Send(cartUpdateCommand);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] CartDeleteCommand cartDeleteCommand)
    {
        var businessResult = await _mediator.Send(cartDeleteCommand);

        return Ok(businessResult);
    }
}