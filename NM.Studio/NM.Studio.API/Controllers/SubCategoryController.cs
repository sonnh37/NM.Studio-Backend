using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.SubCategories;
using NM.Studio.Domain.CQRS.Queries.SubCategories;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
public class SubCategoryController : BaseController
{
    public SubCategoryController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] SubCategoryGetAllQuery subCategoryGetAllQuery)
    {
        var businessResult = await _mediator.Send(subCategoryGetAllQuery);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] SubCategoryGetByIdQuery request)
    {
        var businessResult = await _mediator.Send(request);

        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SubCategoryCreateCommand subCategoryCreateCommand)
    {
        var businessResult = await _mediator.Send(subCategoryCreateCommand);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] SubCategoryUpdateCommand subCategoryUpdateCommand)
    {
        var businessResult = await _mediator.Send(subCategoryUpdateCommand);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] SubCategoryDeleteCommand subCategoryDeleteCommand)
    {
        var businessResult = await _mediator.Send(subCategoryDeleteCommand);

        return Ok(businessResult);
    }
}