using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Services;
using NM.Studio.Domain.CQRS.Queries.Services;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
public class ServiceController : BaseController
{
    public ServiceController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ServiceGetAllQuery serviceGetAllQuery)
    {
        var businessResult = await _mediator.Send(serviceGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] ServiceGetByIdQuery request)
    {
        var businessResult = await _mediator.Send(request);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ServiceCreateCommand serviceCreateCommand)
    {
        var businessResult = await _mediator.Send(serviceCreateCommand);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ServiceUpdateCommand serviceUpdateCommand)
    {
        var businessResult = await _mediator.Send(serviceUpdateCommand);

        return Ok(businessResult);
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ServiceDeleteCommand serviceDeleteCommand)
    {
        var businessResult = await _mediator.Send(serviceDeleteCommand);

        return Ok(businessResult);
    }
}