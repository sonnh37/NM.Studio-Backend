using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.ProductVariants;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class ProductVariantController : BaseController
{
    private readonly IProductVariantService _productVariantService;

    public ProductVariantController(IProductVariantService productVariantService)
    {
        _productVariantService = productVariantService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductVariantCreateCommand request)
    {
        var businessResult = await _productVariantService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductVariantUpdateCommand request)
    {
        var businessResult = await _productVariantService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ProductVariantDeleteCommand request)
    {
        var businessResult = await  _productVariantService.Delete(request);

        return Ok(businessResult);
    }
}