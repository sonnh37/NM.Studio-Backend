using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.MediaFiles;
using NM.Studio.Domain.CQRS.Queries.MediaFiles;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
public class MediaFileController : BaseController
{
    public MediaFileController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] MediaFileGetAllQuery mediaFileGetAllQuery)
    {
        var businessResult = await _mediator.Send(mediaFileGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] MediaFileGetByIdQuery request)
    {
        var businessResult = await _mediator.Send(request);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MediaFileCreateCommand mediaFileCreateCommand)
    {
        var businessResult = await _mediator.Send(mediaFileCreateCommand);

        return Ok(businessResult);
    }


    [HttpPut]
    public async Task<IActionResult> Update([FromBody] MediaFileUpdateCommand mediaFileUpdateCommand)
    {
        var businessResult = await _mediator.Send(mediaFileUpdateCommand);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] MediaFileDeleteCommand mediaFileDeleteCommand)
    {
        var businessResult = await _mediator.Send(mediaFileDeleteCommand);

        return Ok(businessResult);
    }
}