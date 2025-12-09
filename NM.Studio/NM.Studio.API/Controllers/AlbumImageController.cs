using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.CQRS.Commands.AlbumImages;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class AlbumImageController : BaseController
{
    private readonly IAlbumImageService _albumImageService;

    public AlbumImageController(IAlbumImageService albumImageService)
    {
        _albumImageService = albumImageService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AlbumImageCreateCommand request)
    {
        var businessResult = await _albumImageService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AlbumImageUpdateCommand request)
    {
        var businessResult = await _albumImageService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] AlbumImageDeleteCommand request)
    {
        var businessResult = await  _albumImageService.Delete(request);

        return Ok(businessResult);
    }
    
    [HttpPost("list")]
    public async Task<IActionResult> CreateListProductMedia([FromBody] List<AlbumImageCreateCommand> request)
    {
        var businessResult = await _albumImageService.CreateList(request);

        return Ok(businessResult);
    }
    
    [HttpDelete("list")]
    public async Task<IActionResult> DeleteList([FromBody] List<AlbumImageDeleteCommand> request)
    {
        var businessResult = await  _albumImageService.DeleteList(request);

        return Ok(businessResult);
    }
}