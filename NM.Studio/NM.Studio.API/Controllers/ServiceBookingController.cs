using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.ServiceBookings;
using NM.Studio.Domain.CQRS.Queries.ServiceBookings;

namespace NM.Studio.API.Controllers;

public class ServiceBookingController : BaseController
{
    public ServiceBookingController(IMediator mediator) : base(mediator)
    {
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ServiceBookingGetAllQuery serviceBookingGetAllQuery)
    {
        var businessResult = await _mediator.Send(serviceBookingGetAllQuery);

        return Ok(businessResult);
    }

    [Authorize(Roles = "Admin,Staff,Customer")]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] ServiceBookingGetByIdQuery request)
    {
        var businessResult = await _mediator.Send(request);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ServiceBookingCreateCommand serviceBookingCreateCommand)
    {
        var businessResult = await _mediator.Send(serviceBookingCreateCommand);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ServiceBookingUpdateCommand serviceBookingUpdateCommand)
    {
        var businessResult = await _mediator.Send(serviceBookingUpdateCommand);

        return Ok(businessResult);
    }

    [HttpPut("cancel")]
    public async Task<IActionResult> Cancel([FromBody] ServiceBookingCancelCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ServiceBookingDeleteCommand serviceBookingDeleteCommand)
    {
        var businessResult = await _mediator.Send(serviceBookingDeleteCommand);

        return Ok(businessResult);
    }
}