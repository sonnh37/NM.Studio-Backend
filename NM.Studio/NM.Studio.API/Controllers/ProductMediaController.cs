using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.CQRS.Commands.ProductMedias;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class ProductMediaController : BaseController
{
    private readonly IProductMediaService _productMediaService;

    public ProductMediaController(IProductMediaService productMediaService)
    {
        _productMediaService = productMediaService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductMediaCreateCommand request)
    {
        var businessResult = await _productMediaService.CreateOrUpdate(request);

        return Ok(businessResult);
    }
    
    [HttpPost("list")]
    public async Task<IActionResult> CreateListProductMedia([FromBody] List<ProductMediaCreateCommand> request)
    {
        var businessResult = await _productMediaService.CreateList(request);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductMediaUpdateCommand request)
    {
        var businessResult = await _productMediaService.CreateOrUpdate(request);

        return Ok(businessResult);
    }
    
    // [HttpPut("status")]
    // public async Task<IActionResult> UpdateStatus([FromBody] ProductMediaUpdateStatusCommand request)
    // {
    //     var businessResult = await _productMediaService.UpdateStatus(request);
    //
    //     return Ok(businessResult);
    // }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ProductMediaDeleteCommand request)
    {
        var businessResult = await  _productMediaService.Delete(request);

        return Ok(businessResult);
    }
    
    [HttpDelete("list")]
    public async Task<IActionResult> DeleteList([FromBody] List<ProductMediaDeleteCommand> request)
    {
        var businessResult = await  _productMediaService.DeleteList(request);

        return Ok(businessResult);
    }
}