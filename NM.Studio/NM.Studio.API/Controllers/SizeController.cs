using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Sizes;
using NM.Studio.Domain.CQRS.Queries.Sizes;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
public class SizeController : BaseController
{
    public SizeController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] SizeGetAllQuery sizeGetAllQuery)
    {
        var businessResult = await _mediator.Send(sizeGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] SizeGetByIdQuery request)
    {
        var businessResult = await _mediator.Send(request);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SizeCreateCommand sizeCreateCommand)
    {
        var businessResult = await _mediator.Send(sizeCreateCommand);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] SizeUpdateCommand sizeUpdateCommand)
    {
        var businessResult = await _mediator.Send(sizeUpdateCommand);

        return Ok(businessResult);
    }
    

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] SizeDeleteCommand sizeDeleteCommand)
    {
        var businessResult = await _mediator.Send(sizeDeleteCommand);

        return Ok(businessResult);
    }
}