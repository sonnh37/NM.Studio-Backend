using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.Models;

public static class InformationUser
{
    public static User? User { get; set; }
    
    public static string? Origin { get; set; }
    
    public static string? Referer { get; set; }
}