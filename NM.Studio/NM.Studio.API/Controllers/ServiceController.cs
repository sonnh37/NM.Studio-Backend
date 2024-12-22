using NM.Studio.Domain.CQRS.Commands.Services;
using NM.Studio.Domain.CQRS.Queries.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Albums;

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
        var messageResult = await _mediator.Send(serviceGetAllQuery);

        return Ok(messageResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var serviceGetByIdQuery = new ServiceGetByIdQuery
        {
            Id = id
        };
        var messageResult = await _mediator.Send(serviceGetByIdQuery);

        return Ok(messageResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ServiceCreateCommand serviceCreateCommand)
    {
        var messageView = await _mediator.Send(serviceCreateCommand);

        return Ok(messageView);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ServiceUpdateCommand serviceUpdateCommand)
    {
        var messageView = await _mediator.Send(serviceUpdateCommand);

        return Ok(messageView);
    }
    
    [HttpPut("restore")]
    public async Task<IActionResult> UpdateIsDeleted([FromBody] ServiceRestoreCommand command)
    {
        var messageView = await _mediator.Send(command);

        return Ok(messageView);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ServiceDeleteCommand serviceDeleteCommand)
    {
        var messageView = await _mediator.Send(serviceDeleteCommand);

        return Ok(messageView);
    }
}