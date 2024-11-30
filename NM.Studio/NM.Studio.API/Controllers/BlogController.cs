using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.Blogs;
using NM.Studio.Domain.CQRS.Queries.Blogs;

namespace NM.Studio.API.Controllers;

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
        var messageResult = await _mediator.Send(blogGetAllQuery);

        return Ok(messageResult);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var blogGetByIdQuery = new BlogGetByIdQuery
        {
            Id = id
        };
        var messageResult = await _mediator.Send(blogGetByIdQuery);

        return Ok(messageResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BlogCreateCommand blogCreateCommand)
    {
        var messageView = await _mediator.Send(blogCreateCommand);

        return Ok(messageView);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] BlogUpdateCommand blogUpdateCommand)
    {
        var messageView = await _mediator.Send(blogUpdateCommand);

        return Ok(messageView);
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] BlogDeleteCommand blogDeleteCommand)
    {
        var messageView = await _mediator.Send(blogDeleteCommand);

        return Ok(messageView);
    }
}