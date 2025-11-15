using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.CQRS.Commands.Blogs;
using NM.Studio.Domain.Models.CQRS.Queries.Blogs;

namespace NM.Studio.API.Controllers;

// [Authorize(Roles = "Admin,Staff")]
public class BlogController : BaseController
{
    private readonly IBlogService _blogService;
    private readonly ILogger<BlogController> _logger;

    public BlogController(IBlogService blogService, ILogger<BlogController> logger)
    {
        _logger = logger;
        _blogService = blogService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] BlogGetAllQuery request)
    {
        var businessResult = await _blogService.GetAll(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] BlogGetByIdQuery request)
    {
        var businessResult = await _blogService.GetById(request);
        return Ok(businessResult);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BlogCreateCommand request)
    {
        _logger.LogInformation("Blog Create Request: {request}", JsonConvert.SerializeObject(request));
        var businessResult = await _blogService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] BlogUpdateCommand request)
    {
        _logger.LogInformation("Blog Update Request: {request}", JsonConvert.SerializeObject(request));
        var businessResult = await _blogService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] BlogDeleteCommand request)
    {
        var businessResult = await  _blogService.Delete(request);

        return Ok(businessResult);
    }
}