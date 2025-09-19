using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Queries.Users;

namespace NM.Studio.API.Controllers;

// [AllowAnonymous]
public class UserController : BaseController
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] UserGetAllQuery request)
    {
        _logger.LogError($"GetAll process started: {JsonSerializer.Serialize(request)}");
        var businessResult = await _userService.GetAll(request);

        return Ok(businessResult);
    }

    [AllowAnonymous]
    [HttpGet("id")]
    public async Task<IActionResult> GetById([FromQuery] UserGetByIdQuery request)
    {
        var businessResult = await _userService.GetById(request);

        return Ok(businessResult);
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateCommand request)
    {
        var businessResult = await _userService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserUpdateCommand request)
    {
        var businessResult = await _userService.CreateOrUpdate(request);

        return Ok(businessResult);
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpPut("update-cache")]
    public async Task<IActionResult> UpdateUserCache([FromBody] UserUpdateCacheCommand request)
    {
        var businessResult = await _userService.UpdateUserCacheAsync(request);

        return Ok(businessResult);
    }


    [HttpPut("password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UserPasswordCommand userUpdateCommand)
    {
        var businessResult = await _userService.UpdatePassword(userUpdateCommand);

        return Ok(businessResult);
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] UserDeleteCommand userDeleteCommand)
    {
        var businessResult = await _userService.Delete(userDeleteCommand);

        return Ok(businessResult);
    }
}