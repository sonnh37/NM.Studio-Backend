using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Colors;
using NM.Studio.Domain.CQRS.Queries.Colors;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
public class ColorController : BaseController
{
    public ColorController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ColorGetAllQuery colorGetAllQuery)
    {
        var businessResult = await _mediator.Send(colorGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] ColorGetByIdQuery request)
    {
        var businessResult = await _mediator.Send(request);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ColorCreateCommand colorCreateCommand)
    {
        var businessResult = await _mediator.Send(colorCreateCommand);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ColorUpdateCommand colorUpdateCommand)
    {
        var businessResult = await _mediator.Send(colorUpdateCommand);

        return Ok(businessResult);
    }
    

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ColorDeleteCommand colorDeleteCommand)
    {
        var businessResult = await _mediator.Send(colorDeleteCommand);

        return Ok(businessResult);
    }
}