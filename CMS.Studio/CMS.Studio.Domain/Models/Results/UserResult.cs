﻿using CMS.Studio.Domain.Models.Results.Bases;

namespace CMS.Studio.Domain.Models.Results;

public class UserResult : BaseResult
{
    public string? FullName { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime? Dob { get; set; }

    public string? Address { get; set; }

    public string? Gender { get; set; }

    public string Phone { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? RoleName { get; set; }

    public string? Avatar { get; set; }

    public string? Status { get; set; }
}