using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Services;
using NM.Studio.Domain.CQRS.Queries.Services;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
[Route("services")]
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
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var serviceGetByIdQuery = new ServiceGetByIdQuery
        {
            Id = id
        };
        var businessResult = await _mediator.Send(serviceGetByIdQuery);

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

    [HttpPut("restore")]
    public async Task<IActionResult> UpdateIsDeleted([FromBody] ServiceRestoreCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ServiceDeleteCommand serviceDeleteCommand)
    {
        var businessResult = await _mediator.Send(serviceDeleteCommand);

        return Ok(businessResult);
    }
}