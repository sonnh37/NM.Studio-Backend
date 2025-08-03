using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.OrderStatusHistories;
using NM.Studio.Domain.CQRS.Queries.OrderStatusHistories;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class OrderStatusHistoryController : BaseController
{
    public OrderStatusHistoryController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] OrderStatusHistoryGetAllQuery orderStatusHistoryGetAllQuery)
    {
        var businessResult = await _mediator.Send(orderStatusHistoryGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] OrderStatusHistoryGetByIdQuery request)
    {
        var businessResult = await _mediator.Send(request);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderStatusHistoryCreateCommand orderStatusHistoryCreateCommand)
    {
        var businessResult = await _mediator.Send(orderStatusHistoryCreateCommand);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] OrderStatusHistoryUpdateCommand orderStatusHistoryUpdateCommand)
    {
        var businessResult = await _mediator.Send(orderStatusHistoryUpdateCommand);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] OrderStatusHistoryDeleteCommand orderStatusHistoryDeleteCommand)
    {
        var businessResult = await _mediator.Send(orderStatusHistoryDeleteCommand);

        return Ok(businessResult);
    }
}