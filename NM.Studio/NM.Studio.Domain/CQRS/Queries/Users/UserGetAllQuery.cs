using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.CQRS.Queries.Users;

public class UserGetAllQuery : GetAllQuery
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Avatar { get; set; }

    public string? Email { get; set; }

    public DateTimeOffset? Dob { get; set; }

    public string? Address { get; set; }

    public Gender? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Username { get; set; }

    public Role? Role { get; set; }

    public UserStatus? Status { get; set; }

    public string? Preferences { get; set; }
}