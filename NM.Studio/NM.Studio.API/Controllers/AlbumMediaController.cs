using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.AlbumMedias;
using NM.Studio.Domain.CQRS.Queries.Albums;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
public class AlbumMediaController : BaseController
{
    public AlbumMediaController(IMediator mediator) : base(mediator)
    {
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] AlbumGetAllQuery request)
    {
        var businessResult = await _mediator.Send(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] AlbumGetByIdQuery request)
    {
   
        var businessResult = await _mediator.Send(request);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AlbumMediaCreateCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AlbumMediaUpdateCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] AlbumMediaDeleteCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }
}