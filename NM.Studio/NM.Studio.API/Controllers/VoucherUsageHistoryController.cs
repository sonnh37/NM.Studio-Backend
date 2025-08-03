using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.VoucherUsageHistories;
using NM.Studio.Domain.CQRS.Queries.VoucherUsageHistories;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class VoucherUsageHistoryController : BaseController
{
    public VoucherUsageHistoryController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] VoucherUsageHistoryGetAllQuery voucherUsageHistoryGetAllQuery)
    {
        var businessResult = await _mediator.Send(voucherUsageHistoryGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] VoucherUsageHistoryGetByIdQuery request)
    {
        var businessResult = await _mediator.Send(request);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VoucherUsageHistoryCreateCommand voucherUsageHistoryCreateCommand)
    {
        var businessResult = await _mediator.Send(voucherUsageHistoryCreateCommand);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] VoucherUsageHistoryUpdateCommand voucherUsageHistoryUpdateCommand)
    {
        var businessResult = await _mediator.Send(voucherUsageHistoryUpdateCommand);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] VoucherUsageHistoryDeleteCommand voucherUsageHistoryDeleteCommand)
    {
        var businessResult = await _mediator.Send(voucherUsageHistoryDeleteCommand);

        return Ok(businessResult);
    }
}