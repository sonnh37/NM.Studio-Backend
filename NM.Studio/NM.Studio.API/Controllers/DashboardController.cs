using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models;
using NM.Studio.Services;

namespace NM.Studio.API.Controllers;

[Authorize(Roles = "Admin,Staff")]
[Route("dashboard")]
public class DashboardController : BaseController
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }


    [HttpGet("stats")]
    public async Task<IActionResult> GetStats([FromQuery] DashboardGetStatsQuery request)
    {
        var businessResult = await _dashboardService.GetStats(request);
        
        return Ok(businessResult);
    }
}