using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.Enums;

namespace NM.Studio.Domain.CQRS.Commands.Users;

public class UserUpdateCommand : UpdateCommand
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Avatar { get; set; }

    public string? Email { get; set; }

    public DateTime? Dob { get; set; }

    public string? Address { get; set; }

    public Gender? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public Role? Role { get; set; }

    public UserStatus? Status { get; set; }

    public string? Preferences { get; set; }
}