using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.Users;

public class UserCreateCommand : CreateCommand
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get; set; }
    public string? DisplayName { get; set; }
    public Guid? AvatarId { get; set; }
    public string? Email { get; set; }
    public DateTimeOffset? Dob { get; set; }
    public string? Address { get; set; }
    public Gender? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}