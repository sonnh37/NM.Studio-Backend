using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Bookings;
using NM.Studio.Domain.CQRS.Queries.Bookings;

namespace NM.Studio.API.Controllers;

[Route("bookings")]
public class BookingController : BaseController
{
    public BookingController(IMediator mediator) : base(mediator)
    {
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] BookingGetAllQuery bookingGetAllQuery)
    {
        var businessResult = await _mediator.Send(bookingGetAllQuery);

        return Ok(businessResult);
    }

    [Authorize(Roles = "Admin,Staff,Customer")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var bookingGetByIdQuery = new BookingGetByIdQuery
        {
            Id = id
        };
        var businessResult = await _mediator.Send(bookingGetByIdQuery);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookingCreateCommand bookingCreateCommand)
    {
        var businessResult = await _mediator.Send(bookingCreateCommand);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] BookingUpdateCommand bookingUpdateCommand)
    {
        var businessResult = await _mediator.Send(bookingUpdateCommand);

        return Ok(businessResult);
    }
    
    [HttpPut("restore")]
    public async Task<IActionResult> UpdateIsDeleted([FromBody] BookingRestoreCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }
    
    [HttpPut("cancel")]
    public async Task<IActionResult> Cancel([FromBody] BookingCancelCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] BookingDeleteCommand bookingDeleteCommand)
    {
        var businessResult = await _mediator.Send(bookingDeleteCommand);

        return Ok(businessResult);
    }
}