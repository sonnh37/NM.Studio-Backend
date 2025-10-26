using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.CQRS.Commands.MediaBases;
using NM.Studio.Domain.Models.CQRS.Queries.MediaBases;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class MediaBaseController : BaseController
{
    private readonly IMediaBaseService _mediaBaseService;

    public MediaBaseController(IMediaBaseService mediaBaseService)
    {
        _mediaBaseService = mediaBaseService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] MediaBaseGetAllQuery request)
    {
        var businessResult = await _mediaBaseService.GetAll(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] MediaBaseGetByIdQuery request)
    {
        var businessResult = await _mediaBaseService.GetById(request);
        return Ok(businessResult);
    }


    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] MediaBaseDeleteCommand request)
    {
        var businessResult = await  _mediaBaseService.Delete(request);

        return Ok(businessResult);
    }
}