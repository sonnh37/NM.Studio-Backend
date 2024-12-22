using NM.Studio.Domain.Enums;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class UserResult : BaseResult
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Avatar  { get; set; }

    public string? Email { get; set; }

    public DateTime? Dob { get; set; }

    public string? Address { get; set; }

    public Gender? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Username { get; set; }

    public Role? Role { get; set; }

    public UserStatus? Status { get; set; }
    
    public string? Preferences { get; set; } 
}

public class UserLoginResult
{
    public string? Username { get; set; }
    
    public string? FirstName { get; set; }

    public string? ImageUrl { get; set; }

    public string? Email { get; set; }
}