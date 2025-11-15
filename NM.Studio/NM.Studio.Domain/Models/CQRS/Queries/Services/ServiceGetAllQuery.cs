using NM.Studio.Domain.Models.CQRS.Queries.Base;

namespace NM.Studio.Domain.Models.CQRS.Queries.Services;

public class ServiceGetAllQuery : GetAllQuery
{
   public string? Name { get; set; }
   public string? Slug { get; set; }
}