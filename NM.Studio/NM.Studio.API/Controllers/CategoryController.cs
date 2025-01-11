using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Categories;
using NM.Studio.Domain.CQRS.Queries.Categories;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
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
        var businessResult = await _mediator.Send(categoryGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var categoryGetByIdQuery = new CategoryGetByIdQuery
        {
            Id = id
        };
        var businessResult = await _mediator.Send(categoryGetByIdQuery);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryCreateCommand categoryCreateCommand)
    {
        var businessResult = await _mediator.Send(categoryCreateCommand);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CategoryUpdateCommand categoryUpdateCommand)
    {
        var businessResult = await _mediator.Send(categoryUpdateCommand);

        return Ok(businessResult);
    }
    
    [HttpPut("restore")]
    public async Task<IActionResult> UpdateIsDeleted([FromBody] CategoryRestoreCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] CategoryDeleteCommand categoryDeleteCommand)
    {
        var businessResult = await _mediator.Send(categoryDeleteCommand);

        return Ok(businessResult);
    }
}