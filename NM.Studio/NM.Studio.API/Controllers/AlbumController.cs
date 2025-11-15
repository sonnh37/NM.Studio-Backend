using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.CQRS.Commands.Albums;
using NM.Studio.Domain.Models.CQRS.Queries.Albums;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
public class AlbumController : BaseController
{
    private readonly IAlbumService _albumService;
    private readonly ILogger<AlbumController> _logger;


    public AlbumController(IAlbumService albumService, ILogger<AlbumController> logger)
    {
        _logger = logger;
        _albumService = albumService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] AlbumGetAllQuery request)
    {
        var businessResult = await _albumService.GetAll(request);

        return Ok(businessResult);
    }
    
    [AllowAnonymous]
    [HttpGet("by-slug")]
    public async Task<IActionResult> GetAlbumBySlug([FromQuery] AlbumGetBySlugQuery request)
    {
        var businessResult = await _albumService.GetAlbumBySlug(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] AlbumGetByIdQuery request)
    {
        var businessResult = await _albumService.GetById(request);
        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AlbumCreateCommand request)
    {
        _logger.LogInformation("Album Create Request: {request}", JsonConvert.SerializeObject(request));
        var businessResult = await _albumService.CreateOrUpdate(request);

        return Ok(businessResult);
    }
    
    [HttpPost("images")]
    public async Task<IActionResult> CreateImages([FromBody] AlbumWithImagesCreateCommand request)
    {
        _logger.LogInformation("Album Create Request: {request}", JsonConvert.SerializeObject(request));
        var businessResult = await _albumService.CreateWithImages(request);

        return Ok(businessResult);
    }
    
    [HttpPut("images/set-cover")]
    public async Task<IActionResult> SetCoverAlbum([FromBody] AlbumSetCoverUpdateCommand request)
    {
        _logger.LogInformation("Album Create Request: {request}", JsonConvert.SerializeObject(request));
        var businessResult = await _albumService.SetCoverAlbum(request);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AlbumUpdateCommand request)
    {
        _logger.LogInformation("Album update Request: {request}", JsonConvert.SerializeObject(request));
        var businessResult = await _albumService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] AlbumDeleteCommand request)
    {
        var businessResult = await  _albumService.Delete(request);

        return Ok(businessResult);
    }
}