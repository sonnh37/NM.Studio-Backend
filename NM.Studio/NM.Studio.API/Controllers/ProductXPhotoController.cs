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
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductXPhotoUpdateCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ProductXPhotoDeleteCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }
}