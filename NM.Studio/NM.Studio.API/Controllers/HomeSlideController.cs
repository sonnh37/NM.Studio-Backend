using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.CQRS.Commands.HomeSlides;
using NM.Studio.Domain.Models.CQRS.Queries.HomeSlides;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class HomeSlideController : BaseController
{
    private readonly IHomeSlideService _homeSlideService;

    public HomeSlideController(IHomeSlideService homeSlideService)
    {
        _homeSlideService = homeSlideService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] HomeSlideGetAllQuery request)
    {
        var businessResult = await _homeSlideService.GetAll(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] HomeSlideGetByIdQuery request)
    {
        var businessResult = await _homeSlideService.GetById(request);
        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] HomeSlideCreateCommand request)
    {
        var businessResult = await _homeSlideService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] HomeSlideUpdateCommand request)
    {
        var businessResult = await _homeSlideService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] HomeSlideDeleteCommand request)
    {
        var businessResult = await  _homeSlideService.Delete(request);

        return Ok(businessResult);
    }
}