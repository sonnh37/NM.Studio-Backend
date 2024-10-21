﻿using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Enums;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Domain.CQRS.Queries.Users;

public class UserGetAllQuery : GetAllQuery
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? ImageUrl { get; set; }

    public string? Email { get; set; }

    public DateTime? Dob { get; set; }

    public string? Address { get; set; }

    public Gender? Gender { get; set; }

    public string? Phone { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public Role? Role { get; set; }

    public string? Avatar { get; set; }

    public UserStatus? Status { get; set; }
}