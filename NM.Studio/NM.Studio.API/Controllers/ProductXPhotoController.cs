using NM.Studio.Domain.CQRS.Commands.ProductXPhotos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
[Route("products/productXPhotos")]
public class ProductXPhotoController : BaseController
{
    public ProductXPhotoController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductXPhotoCreateCommand command)
    {
        var messageView = await _mediator.Send(command);

        return Ok(messageView);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductXPhotoUpdateCommand command)
    {
        var messageView = await _mediator.Send(command);

        return Ok(messageView);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ProductXPhotoDeleteCommand command)
    {
        var messageView = await _mediator.Send(command);

        return Ok(messageView);
    }
}