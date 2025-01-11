using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Blogs;
using NM.Studio.Domain.CQRS.Queries.Blogs;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
[Route("blogs")]
public class BlogController : BaseController
{
    public BlogController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] BlogGetAllQuery blogGetAllQuery)
    {
        var businessResult = await _mediator.Send(blogGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var blogGetByIdQuery = new BlogGetByIdQuery
        {
            Id = id
        };
        var businessResult = await _mediator.Send(blogGetByIdQuery);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BlogCreateCommand blogCreateCommand)
    {
        var businessResult = await _mediator.Send(blogCreateCommand);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] BlogUpdateCommand blogUpdateCommand)
    {
        var businessResult = await _mediator.Send(blogUpdateCommand);

        return Ok(businessResult);
    }
    
    [HttpPut("restore")]
    public async Task<IActionResult> UpdateIsDeleted([FromBody] BlogRestoreCommand command)
    {
        var businessResult = await _mediator.Send(command);

        return Ok(businessResult);
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] BlogDeleteCommand blogDeleteCommand)
    {
        var businessResult = await _mediator.Send(blogDeleteCommand);

        return Ok(businessResult);
    }
}