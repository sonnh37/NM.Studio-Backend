using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Bookings;
using NM.Studio.Domain.CQRS.Queries.Bookings;

namespace NM.Studio.API.Controllers;

//[Authorize]
[Route("bookings")]
public class BookingController : BaseController
{
    public BookingController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] BookingGetAllQuery bookingGetAllQuery)
    {
        var messageResult = await _mediator.Send(bookingGetAllQuery);

        return Ok(messageResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var bookingGetByIdQuery = new BookingGetByIdQuery
        {
            Id = id
        };
        var messageResult = await _mediator.Send(bookingGetByIdQuery);

        return Ok(messageResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookingCreateCommand bookingCreateCommand)
    {
        var messageView = await _mediator.Send(bookingCreateCommand);

        return Ok(messageView);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] BookingUpdateCommand bookingUpdateCommand)
    {
        var messageView = await _mediator.Send(bookingUpdateCommand);

        return Ok(messageView);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] BookingDeleteCommand bookingDeleteCommand)
    {
        var messageView = await _mediator.Send(bookingDeleteCommand);

        return Ok(messageView);
    }
}