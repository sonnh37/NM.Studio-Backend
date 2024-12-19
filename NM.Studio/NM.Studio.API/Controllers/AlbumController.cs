using NM.Studio.Domain.CQRS.Commands.Albums;
using NM.Studio.Domain.CQRS.Queries.Albums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;

namespace NM.Studio.API.Controllers;

[Authorize]
[Route("albums")]
public class AlbumController : BaseController
{
    public AlbumController(IMediator mediator) : base(mediator)
    {
    }

    // [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] AlbumGetAllQuery albumGetAllQuery)
    {
        var accessToken = Request.Cookies["accessToken"];
        var messageResult = await _mediator.Send(albumGetAllQuery);

        return Ok(messageResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var albumGetByIdQuery = new AlbumGetByIdQuery
        {
            Id = id
        };
        var messageResult = await _mediator.Send(albumGetByIdQuery);

        return Ok(messageResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AlbumCreateCommand albumCreateCommand)
    {
        var messageView = await _mediator.Send(albumCreateCommand);

        return Ok(messageView);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AlbumUpdateCommand albumUpdateCommand)
    {
        var messageView = await _mediator.Send(albumUpdateCommand);

        return Ok(messageView);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] AlbumDeleteCommand albumDeleteCommand)
    {
        var messageView = await _mediator.Send(albumDeleteCommand);

        return Ok(messageView);
    }
}