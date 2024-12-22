using NM.Studio.Domain.CQRS.Commands.Products;
using NM.Studio.Domain.CQRS.Queries.Products;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
[Route("products")]
public class ProductController : BaseController
{
    public ProductController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ProductGetAllQuery productGetAllQuery)
    {
        var messageResult = await _mediator.Send(productGetAllQuery);

        return Ok(messageResult);
    }
    
    [AllowAnonymous]
    [HttpGet("representative-by-category")]
    public async Task<IActionResult> GetRepresentativeByCategory([FromQuery] ProductRepresentativeByCategoryQuery query)
    {
        var messageResult = await _mediator.Send(query);

        return Ok(messageResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var productGetByIdQuery = new ProductGetByIdQuery
        {
            Id = id
        };
        var messageResult = await _mediator.Send(productGetByIdQuery);

        return Ok(messageResult);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateCommand productCreateCommand)
    {
        var messageView = await _mediator.Send(productCreateCommand);

        return Ok(messageView);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductUpdateCommand productUpdateCommand)
    {
        var messageView = await _mediator.Send(productUpdateCommand);

        return Ok(messageView);
    }
    
    [HttpPut("restore")]
    public async Task<IActionResult> UpdateIsDeleted([FromBody] ProductRestoreCommand command)
    {
        var messageView = await _mediator.Send(command);

        return Ok(messageView);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ProductDeleteCommand productDeleteCommand)
    {
        var messageView = await _mediator.Send(productDeleteCommand);

        return Ok(messageView);
    }
}