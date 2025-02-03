using MediatR;
using NM.Studio.Domain.Models.Responses;

namespace NM.Studio.Domain.Models.Requests;

public class DashboardGetStatsQuery : IRequest<BusinessResult>
{
    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}