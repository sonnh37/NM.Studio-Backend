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
        var businessResult = await _mediator.Send(photoGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var photoGetByIdQuery = new PhotoGetByIdQuery
        {
            Id = id
        };
        var businessResult = await _mediator.Send(photoGetByIdQuery);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PhotoCreateCommand photoCreateCommand)
    {
        var businessResult = await _mediator.Send(photoCreateCommand);

        return Ok(businessResult);
    }


    [HttpPut]
    public async Task<IActionResult> Update([FromBody] PhotoUpdateCommand photoUpdateCommand)
    {
        var businessResult = await _mediator.Send(photoUpdateCommand);

        return Ok(businessResult);
    }
    
    [HttpPut("restore")]
    public async Task<IActionResult> UpdateIsDeleted([FromBody] PhotoRestoreCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] PhotoDeleteCommand photoDeleteCommand)
    {
        var businessResult = await _mediator.Send(photoDeleteCommand);

        return Ok(businessResult);
    }
}