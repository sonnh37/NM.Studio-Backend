using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Albums;
using NM.Studio.Domain.CQRS.Queries.Albums;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class AlbumController : BaseController
{
    public AlbumController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] AlbumGetAllQuery albumGetAllQuery)
    {
        var businessResult = await _mediator.Send(albumGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] AlbumGetByIdQuery request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
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

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] AlbumDeleteCommand albumDeleteCommand)
    {
        var businessResult = await _mediator.Send(albumDeleteCommand);

        return Ok(businessResult);
    }
}