using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using System.Security.Claims;

namespace NM.Studio.Domain.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AuthenticationMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                  ?? context.User.FindFirst("Id")?.Value;

                if (!string.IsNullOrEmpty(userIdClaim))
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                        if (Guid.TryParse(userIdClaim, out var userId))
                        {
                            var user = await unitOfWork.UserRepository.GetById(userId);

                            if (user != null)
                            {
                                context.Items["CurrentUser"] = user;
                                context.Items["CurrentUserId"] = userId;
                            }
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}