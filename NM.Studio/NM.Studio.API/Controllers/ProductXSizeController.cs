using NM.Studio.Domain.CQRS.Commands.ProductXSizes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Queries.ProductXSizes;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
[Route("products/productXSizes")]
public class ProductXSizeController : BaseController
{
    public ProductXSizeController(IMediator mediator) : base(mediator)
    {
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ProductXSizeGetAllQuery productXSizeGetAllQuery)
    {
        var businessResult = await _mediator.Send(productXSizeGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var productXSizeGetByIdQuery = new ProductXSizeGetByIdQuery
        {
            Id = id
        };
        var businessResult = await _mediator.Send(productXSizeGetByIdQuery);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductXSizeCreateCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductXSizeUpdateCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ProductXSizeDeleteCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }
}