using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.CartItems;
using NM.Studio.Domain.CQRS.Queries.CartItems;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class CartItemController : BaseController
{
    public CartItemController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] CartItemGetAllQuery cartItemGetAllQuery)
    {
        var businessResult = await _mediator.Send(cartItemGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] CartItemGetByIdQuery request)
    {
        var businessResult = await _mediator.Send(request);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CartItemCreateCommand cartItemCreateCommand)
    {
        var businessResult = await _mediator.Send(cartItemCreateCommand);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CartItemUpdateCommand cartItemUpdateCommand)
    {
        var businessResult = await _mediator.Send(cartItemUpdateCommand);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] CartItemDeleteCommand cartItemDeleteCommand)
    {
        var businessResult = await _mediator.Send(cartItemDeleteCommand);

        return Ok(businessResult);
    }
}