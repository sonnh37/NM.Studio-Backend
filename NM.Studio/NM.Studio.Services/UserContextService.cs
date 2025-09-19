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
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return null;
        if (Guid.TryParse(userId, out var guid))
            return guid;
        return null;
    }

    public string? GetEmail()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
    }

    public string? GetUserName()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
    }

    public string? GetPhoneNumber()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.MobilePhone)?.Value;
    }

    public string? GetRole()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
    }

    public string? GetDisplayName()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst("DisplayName")?.Value;
    }
}