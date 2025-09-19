namespace NM.Studio.Domain.Models;

public class DashboardGetStatsQuery
{
    public DateTimeOffset startDate { get; set; }
    public DateTimeOffset endDate { get; set; }
    
}