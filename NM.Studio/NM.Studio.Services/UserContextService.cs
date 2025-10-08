using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NM.Studio.Domain.Contracts.Services;

namespace NM.Studio.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetUserId()
    {
        if(!IsLoggedIn()) return null;
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("Id")?.Value;
        if (userId == null)
            return null;
        if (Guid.TryParse(userId, out var guid))
            return guid;
        return null;
    }

    public string? GetEmail()
    {
        return null;
        // return _httpContextAccessor.HttpContext?.User?.FindFirst(m => m.Type)?.Value;
    }

    public string? GetUserName()
    {
        return null;
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
    }

    public string? GetPhoneNumber()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst("Phone")?.Value;
    }

    public string? GetRole()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst("Role")?.Value;
    }

    public string? GetDisplayName()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst("DisplayName")?.Value;
    }
    
    public bool IsLoggedIn()
    {
        return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}