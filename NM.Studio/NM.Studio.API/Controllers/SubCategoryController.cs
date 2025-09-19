using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.SubCategories;
using NM.Studio.Domain.CQRS.Queries.SubCategories;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class SubCategoryController : BaseController
{
    private readonly ISubCategoryService _subCategoryService;

    public SubCategoryController(ISubCategoryService subCategoryService)
    {
        _subCategoryService = subCategoryService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] SubCategoryGetAllQuery request)
    {
        var businessResult = await _subCategoryService.GetAll(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] SubCategoryGetByIdQuery request)
    {
        var businessResult = await _subCategoryService.GetById(request);
        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SubCategoryCreateCommand request)
    {
        var businessResult = await _subCategoryService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] SubCategoryUpdateCommand request)
    {
        var businessResult = await _subCategoryService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] SubCategoryDeleteCommand request)
    {
        var businessResult = await _subCategoryService.Delete(request);

        return Ok(businessResult);
    }
}