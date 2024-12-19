using NM.Studio.Domain.CQRS.Commands.AlbumXPhotos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Enums;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
[Route("albums/albumXPhotos")]
public class AlbumXPhotoController : BaseController
{
    public AlbumXPhotoController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AlbumXPhotoCreateCommand command)
    {
        var messageView = await _mediator.Send(command);

        return Ok(messageView);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AlbumXPhotoUpdateCommand command)
    {
        var messageView = await _mediator.Send(command);

        return Ok(messageView);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] AlbumXPhotoDeleteCommand command)
    {
        var messageView = await _mediator.Send(command);

        return Ok(messageView);
    }
}