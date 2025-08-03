using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Vouchers;
using NM.Studio.Domain.CQRS.Queries.Vouchers;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class VoucherController : BaseController
{
    public VoucherController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] VoucherGetAllQuery voucherGetAllQuery)
    {
        var businessResult = await _mediator.Send(voucherGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] VoucherGetByIdQuery request)
    {
        var businessResult = await _mediator.Send(request);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VoucherCreateCommand voucherCreateCommand)
    {
        var businessResult = await _mediator.Send(voucherCreateCommand);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] VoucherUpdateCommand voucherUpdateCommand)
    {
        var businessResult = await _mediator.Send(voucherUpdateCommand);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] VoucherDeleteCommand voucherDeleteCommand)
    {
        var businessResult = await _mediator.Send(voucherDeleteCommand);

        return Ok(businessResult);
    }
}