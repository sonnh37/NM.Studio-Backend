using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.CQRS.Commands.Images;
using NM.Studio.Domain.Models.CQRS.Queries.Images;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class ImageController : BaseController
{
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ImageGetAllQuery request)
    {
        var businessResult = await _imageService.GetAll(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] ImageGetByIdQuery request)
    {
        var businessResult = await _imageService.GetById(request);
        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ImageCreateCommand request)
    {
        var businessResult = await _imageService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ImageUpdateCommand request)
    {
        var businessResult = await _imageService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] ImageDeleteCommand request)
    {
        var businessResult = await  _imageService.Delete(request);

        return Ok(businessResult);
    }
}