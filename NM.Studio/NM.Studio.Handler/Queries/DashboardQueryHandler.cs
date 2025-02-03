// using NM.Studio.Domain.Contracts.Services;
// using NM.Studio.Domain.CQRS.Queries.Dashboards;
// using NM.Studio.Domain.Models.Responses;
// using NM.Studio.Domain.Models.Results;
// using MediatR;
// using NM.Studio.Domain.Models.Requests;
//
// namespace NM.Studio.Handler.Queries;
//
// public class DashboardQueryHandler :
//     IRequestHandler<DashboardGetStatsQuery, BusinessResult>,
// {
//     protected readonly IDashboardService _dashboardService;
//
//     public DashboardQueryHandler(IDashboardService dashboardService)
//     {
//         _dashboardService = dashboardService;
//     }
//
//     public async Task<BusinessResult> Handle(DashboardGetAllQuery request,
//         CancellationToken cancellationToken)
//     {
//         return await _dashboardService.GetAll<DashboardResult>(request);
//     }
//
//     public async Task<BusinessResult> Handle(DashboardGetByIdQuery request, CancellationToken cancellationToken)
//     {
//         return await _dashboardService.GetById<DashboardResult>(request.Id);
//     }
// }