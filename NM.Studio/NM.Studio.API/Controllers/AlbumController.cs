using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Albums;
using NM.Studio.Domain.CQRS.Queries.Albums;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
[Route("albums")]
public class AlbumController : BaseController
{
    public AlbumController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] AlbumGetAllQuery albumGetAllQuery)
    {
        var accessToken = Request.Cookies["accessToken"];
        var businessResult = await _mediator.Send(albumGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var albumGetByIdQuery = new AlbumGetByIdQuery
        {
            Id = id
        };
        var businessResult = await _mediator.Send(albumGetByIdQuery);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AlbumCreateCommand albumCreateCommand)
    {
        var businessResult = await _mediator.Send(albumCreateCommand);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AlbumUpdateCommand albumUpdateCommand)
    {
        var businessResult = await _mediator.Send(albumUpdateCommand);

        return Ok(businessResult);
    }
    
    [HttpPut("restore")]
    public async Task<IActionResult> UpdateIsDeleted([FromBody] AlbumRestoreCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] AlbumDeleteCommand albumDeleteCommand)
    {
        var businessResult = await _mediator.Send(albumDeleteCommand);

        return Ok(businessResult);
    }
}