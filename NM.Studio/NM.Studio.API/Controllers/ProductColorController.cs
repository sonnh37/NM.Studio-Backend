using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.ProductColors;
using NM.Studio.Domain.CQRS.Queries.ProductColors;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
public class ProductColorController : BaseController
{
    public ProductColorController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ProductColorGetAllQuery productColorGetAllQuery)
    {
        var businessResult = await _mediator.Send(productColorGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] ProductColorGetByIdQuery request)
    {
        var businessResult = await _mediator.Send(request);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductColorCreateCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductColorUpdateCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ProductColorDeleteCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }
}