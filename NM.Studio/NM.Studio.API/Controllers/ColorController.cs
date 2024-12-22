using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Categories;
using NM.Studio.Domain.CQRS.Commands.Colors;
using NM.Studio.Domain.CQRS.Queries.Categories;
using NM.Studio.Domain.CQRS.Queries.Colors;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
[Route("colors")]
public class ColorController : BaseController
{
    public ColorController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ColorGetAllQuery colorGetAllQuery)
    {
        var messageResult = await _mediator.Send(colorGetAllQuery);

        return Ok(messageResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var colorGetByIdQuery = new ColorGetByIdQuery
        {
            Id = id
        };
        var messageResult = await _mediator.Send(colorGetByIdQuery);

        return Ok(messageResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ColorCreateCommand colorCreateCommand)
    {
        var messageView = await _mediator.Send(colorCreateCommand);

        return Ok(messageView);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ColorUpdateCommand colorUpdateCommand)
    {
        var messageView = await _mediator.Send(colorUpdateCommand);

        return Ok(messageView);
    }
    
    [HttpPut("restore")]
    public async Task<IActionResult> UpdateIsDeleted([FromBody] ColorRestoreCommand command)
    {
        var messageView = await _mediator.Send(command);

        return Ok(messageView);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ColorDeleteCommand colorDeleteCommand)
    {
        var messageView = await _mediator.Send(colorDeleteCommand);

        return Ok(messageView);
    }
}