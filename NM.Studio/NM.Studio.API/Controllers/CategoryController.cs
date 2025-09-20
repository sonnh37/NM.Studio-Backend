using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.CQRS.Commands.Categories;
using NM.Studio.Domain.Models.CQRS.Queries.Categories;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] CategoryGetAllQuery request)
    {
        var businessResult = await _categoryService.GetAll(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] CategoryGetByIdQuery request)
    {
        var businessResult = await _categoryService.GetById(request);
        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryCreateCommand request)
    {
        var businessResult = await _categoryService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CategoryUpdateCommand request)
    {
        var businessResult = await _categoryService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] CategoryDeleteCommand request)
    {
        var businessResult = await  _categoryService.Delete(request);

        return Ok(businessResult);
    }
}