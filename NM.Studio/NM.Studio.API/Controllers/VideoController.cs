using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.CQRS.Commands.Videos;
using NM.Studio.Domain.Models.CQRS.Queries.Videos;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class VideoController : BaseController
{
    private readonly IVideoService _videoService;

    public VideoController(IVideoService videoService)
    {
        _videoService = videoService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] VideoGetAllQuery request)
    {
        var businessResult = await _videoService.GetAll(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] VideoGetByIdQuery request)
    {
        var businessResult = await _videoService.GetById(request);
        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VideoCreateCommand request)
    {
        var businessResult = await _videoService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] VideoUpdateCommand request)
    {
        var businessResult = await _videoService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] VideoDeleteCommand request)
    {
        var businessResult = await  _videoService.Delete(request);

        return Ok(businessResult);
    }
}