using NM.Studio.Domain.CQRS.Commands.ProductXColors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Queries.ProductXColors;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
[Route("products/productXColors")]
public class ProductXColorController : BaseController
{
    public ProductXColorController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ProductXColorGetAllQuery productXColorGetAllQuery)
    {
        var businessResult = await _mediator.Send(productXColorGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var productXColorGetByIdQuery = new ProductXColorGetByIdQuery
        {
            Id = id
        };
        var businessResult = await _mediator.Send(productXColorGetByIdQuery);

        return Ok(businessResult);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductXColorCreateCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductXColorUpdateCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ProductXColorDeleteCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }
}