﻿using CMS.Studio.API.Controllers.Base;
using CMS.Studio.Domain.CQRS.Commands.OutfitXPhotos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Studio.API.Controllers;

[Route("outfits/outfitXPhotos")]
public class OutfitXPhotoController : BaseController
{
    public OutfitXPhotoController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OutfitXPhotoCreateCommand command)
    {
        var messageView = await _mediator.Send(command);

        return Ok(messageView);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] OutfitXPhotoUpdateCommand command)
    {
        var messageView = await _mediator.Send(command);

        return Ok(messageView);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] OutfitXPhotoDeleteCommand command)
    {
        var messageView = await _mediator.Send(command);

        return Ok(messageView);
    }
}