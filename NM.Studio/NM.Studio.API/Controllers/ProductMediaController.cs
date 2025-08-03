using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.ProductMedias;
using NM.Studio.Domain.CQRS.Queries.ProductMedias;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
public class ProductMediaController : BaseController
{
    public ProductMediaController(IMediator mediator) : base(mediator)
    {
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ProductMediaGetAllQuery request)
    {
        var businessResult = await _mediator.Send(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] ProductMediaGetByIdQuery request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductMediaCreateCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductMediaUpdateCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ProductMediaDeleteCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }
}