using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Categories;
using NM.Studio.Domain.CQRS.Queries.Categories;

namespace NM.Studio.API.Controllers;

//[Authorize]
[Route("categories")]
public class CategoryController : BaseController
{
    public CategoryController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] CategoryGetAllQuery categoryGetAllQuery)
    {
        var messageResult = await _mediator.Send(categoryGetAllQuery);

        return Ok(messageResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var categoryGetByIdQuery = new CategoryGetByIdQuery
        {
            Id = id
        };
        var messageResult = await _mediator.Send(categoryGetByIdQuery);

        return Ok(messageResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryCreateCommand categoryCreateCommand)
    {
        var messageView = await _mediator.Send(categoryCreateCommand);

        return Ok(messageView);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CategoryUpdateCommand categoryUpdateCommand)
    {
        var messageView = await _mediator.Send(categoryUpdateCommand);

        return Ok(messageView);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] CategoryDeleteCommand categoryDeleteCommand)
    {
        var messageView = await _mediator.Send(categoryDeleteCommand);

        return Ok(messageView);
    }
}