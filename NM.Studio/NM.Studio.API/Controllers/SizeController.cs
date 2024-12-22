using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Categories;
using NM.Studio.Domain.CQRS.Commands.Sizes;
using NM.Studio.Domain.CQRS.Queries.Categories;
using NM.Studio.Domain.CQRS.Queries.Sizes;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
[Route("sizes")]
public class SizeController : BaseController
{
    public SizeController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] SizeGetAllQuery sizeGetAllQuery)
    {
        var messageResult = await _mediator.Send(sizeGetAllQuery);

        return Ok(messageResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var sizeGetByIdQuery = new SizeGetByIdQuery
        {
            Id = id
        };
        var messageResult = await _mediator.Send(sizeGetByIdQuery);

        return Ok(messageResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SizeCreateCommand sizeCreateCommand)
    {
        var messageView = await _mediator.Send(sizeCreateCommand);

        return Ok(messageView);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] SizeUpdateCommand sizeUpdateCommand)
    {
        var messageView = await _mediator.Send(sizeUpdateCommand);

        return Ok(messageView);
    }
    
    [HttpPut("restore")]
    public async Task<IActionResult> UpdateIsDeleted([FromBody] SizeRestoreCommand command)
    {
        var messageView = await _mediator.Send(command);

        return Ok(messageView);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] SizeDeleteCommand sizeDeleteCommand)
    {
        var messageView = await _mediator.Send(sizeDeleteCommand);

        return Ok(messageView);
    }
}