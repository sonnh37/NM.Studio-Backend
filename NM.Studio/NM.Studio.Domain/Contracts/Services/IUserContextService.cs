namespace NM.Studio.Domain.Contracts.Services;

public interface IUserContextService
{
    Guid? GetUserId();
    string? GetEmail();

    string? GetUserName();

    string? GetPhoneNumber();

    string? GetRole();

    string? GetDisplayName();
}