using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Payments;
using NM.Studio.Domain.CQRS.Queries.Payments;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class PaymentController : BaseController
{
    public PaymentController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaymentGetAllQuery paymentGetAllQuery)
    {
        var businessResult = await _mediator.Send(paymentGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] PaymentGetByIdQuery request)
    {
        var businessResult = await _mediator.Send(request);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PaymentCreateCommand paymentCreateCommand)
    {
        var businessResult = await _mediator.Send(paymentCreateCommand);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] PaymentUpdateCommand paymentUpdateCommand)
    {
        var businessResult = await _mediator.Send(paymentUpdateCommand);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] PaymentDeleteCommand paymentDeleteCommand)
    {
        var businessResult = await _mediator.Send(paymentDeleteCommand);

        return Ok(businessResult);
    }
}