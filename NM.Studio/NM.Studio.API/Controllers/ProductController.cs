using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Products;
using NM.Studio.Domain.CQRS.Queries.Products;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class ProductController : BaseController
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    
    [AllowAnonymous]
    [HttpGet("representative-by-category")]
    public async Task<IActionResult> GetRepresentativeByCategory([FromQuery] ProductRepresentativeByCategoryQuery query)
    {
        var businessResult = await _productService.GetRepresentativeByCategory(query);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ProductGetAllQuery request)
    {
        var businessResult = await _productService.GetAll(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] ProductGetByIdQuery request)
    {
        var businessResult = await _productService.GetById(request);
        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateCommand request)
    {
        var businessResult = await _productService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductUpdateCommand request)
    {
        var businessResult = await _productService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ProductDeleteCommand request)
    {
        var businessResult = await  _productService.Delete(request);

        return Ok(businessResult);
    }
}