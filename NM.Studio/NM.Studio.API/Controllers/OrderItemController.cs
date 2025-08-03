using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.OrderItems;
using NM.Studio.Domain.CQRS.Queries.OrderItems;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class OrderItemController : BaseController
{
    public OrderItemController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] OrderItemGetAllQuery orderItemGetAllQuery)
    {
        var businessResult = await _mediator.Send(orderItemGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] OrderItemGetByIdQuery request)
    {
        var businessResult = await _mediator.Send(request);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderItemCreateCommand orderItemCreateCommand)
    {
        var businessResult = await _mediator.Send(orderItemCreateCommand);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] OrderItemUpdateCommand orderItemUpdateCommand)
    {
        var businessResult = await _mediator.Send(orderItemUpdateCommand);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] OrderItemDeleteCommand orderItemDeleteCommand)
    {
        var businessResult = await _mediator.Send(orderItemDeleteCommand);

        return Ok(businessResult);
    }
}