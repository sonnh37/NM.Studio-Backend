using NM.Studio.Domain.CQRS.Commands.Photos;
using NM.Studio.Domain.CQRS.Queries.Photos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
[Route("photos")]
public class PhotoController : BaseController
{
    public PhotoController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PhotoGetAllQuery photoGetAllQuery)
    {
        var messageResult = await _mediator.Send(photoGetAllQuery);

        return Ok(messageResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var photoGetByIdQuery = new PhotoGetByIdQuery
        {
            Id = id
        };
        var messageResult = await _mediator.Send(photoGetByIdQuery);

        return Ok(messageResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PhotoCreateCommand photoCreateCommand)
    {
        var messageView = await _mediator.Send(photoCreateCommand);

        return Ok(messageView);
    }


    [HttpPut]
    public async Task<IActionResult> Update([FromBody] PhotoUpdateCommand photoUpdateCommand)
    {
        var messageView = await _mediator.Send(photoUpdateCommand);

        return Ok(messageView);
    }
    
    [HttpPut("restore")]
    public async Task<IActionResult> UpdateIsDeleted([FromBody] PhotoRestoreCommand command)
    {
        var messageView = await _mediator.Send(command);

        return Ok(messageView);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] PhotoDeleteCommand photoDeleteCommand)
    {
        var messageView = await _mediator.Send(photoDeleteCommand);

        return Ok(messageView);
    }
}