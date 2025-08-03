using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Orders;
using NM.Studio.Domain.CQRS.Queries.Orders;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class OrderController : BaseController
{
    public OrderController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] OrderGetAllQuery orderGetAllQuery)
    {
        var businessResult = await _mediator.Send(orderGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] OrderGetByIdQuery request)
    {
   
        var businessResult = await _mediator.Send(request);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderCreateCommand orderCreateCommand)
    {
        var businessResult = await _mediator.Send(orderCreateCommand);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] OrderUpdateCommand orderUpdateCommand)
    {
        var businessResult = await _mediator.Send(orderUpdateCommand);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] OrderDeleteCommand orderDeleteCommand)
    {
        var businessResult = await _mediator.Send(orderDeleteCommand);

        return Ok(businessResult);
    }
}