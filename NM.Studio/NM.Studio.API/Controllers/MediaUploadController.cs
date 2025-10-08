using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.Requests;

namespace NM.Studio.API.Controllers;

[Authorize]
public class MediaUploadController : BaseController
{
    private readonly IMediaUploadService _mediaUploadService;

    public MediaUploadController(IMediaUploadService mediaUploadService)
    {
        _mediaUploadService = mediaUploadService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadCv([FromForm] FileUploadRequest request)
    {
        var businessResult = await _mediaUploadService.UploadFile(request);

        return Ok(businessResult);
    }
    
    [HttpDelete("{src}")]
    public async Task<IActionResult> DeleteFileAsync([FromQuery] string src)
    {
        var businessResult = await _mediaUploadService.DeleteFileAsync(src);

        return Ok(businessResult);
    }
}