using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Products;
using NM.Studio.Domain.CQRS.Queries.Products;

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
        var businessResult = await _mediator.Send(productGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("representative-by-category")]
    public async Task<IActionResult> GetRepresentativeByCategory([FromQuery] ProductRepresentativeByCategoryQuery query)
    {
        var businessResult = await _mediator.Send(query);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var productGetByIdQuery = new ProductGetByIdQuery
        {
            Id = id
        };
        var businessResult = await _mediator.Send(productGetByIdQuery);

        return Ok(businessResult);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateCommand productCreateCommand)
    {
        var businessResult = await _mediator.Send(productCreateCommand);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductUpdateCommand productUpdateCommand)
    {
        var businessResult = await _mediator.Send(productUpdateCommand);

        return Ok(businessResult);
    }

    [HttpPut("restore")]
    public async Task<IActionResult> UpdateIsDeleted([FromBody] ProductRestoreCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ProductDeleteCommand productDeleteCommand)
    {
        var businessResult = await _mediator.Send(productDeleteCommand);

        return Ok(businessResult);
    }
}