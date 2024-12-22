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
        var messageResult = await _mediator.Send(productXSizeGetAllQuery);

        return Ok(messageResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var productXSizeGetByIdQuery = new ProductXSizeGetByIdQuery
        {
            Id = id
        };
        var messageResult = await _mediator.Send(productXSizeGetByIdQuery);

        return Ok(messageResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductXSizeCreateCommand command)
    {
        var messageView = await _mediator.Send(command);

        return Ok(messageView);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductXSizeUpdateCommand command)
    {
        var messageView = await _mediator.Send(command);

        return Ok(messageView);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ProductXSizeDeleteCommand command)
    {
        var messageView = await _mediator.Send(command);

        return Ok(messageView);
    }
}