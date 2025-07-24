using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NM.Studio.Domain.Contracts.Services;

namespace NM.Studio.API.Extensions;

public static class ConfigExtensions
{
    public static void AddCORS(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            var frontendDomains = Environment.GetEnvironmentVariable("FRONTEND_DOMAIN")?.Split(',');

            options.AddPolicy("AllowSpecificOrigins", builder =>
            {
                if (frontendDomains != null)
                    builder.WithOrigins(frontendDomains) // Thêm các domain của frontend
                        .AllowCredentials() // Cho phép gửi cookie
                        .AllowAnyHeader()
                        .AllowAnyMethod();
            });
        });
    }
    
    public static void AddAuth(this IServiceCollection services)
    {
        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = "Role",

                    IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
                    {
                        var httpContextAccessor =
                            services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>();

                        var authService = httpContextAccessor.HttpContext?.RequestServices.GetRequiredService<IAuthService>();
                        if (authService == null) throw new SecurityTokenException("AuthService not available.");

                        var rsa = authService.GetRSAKeyFromTokenAsync(token, kid).Result;
                        return new List<SecurityKey> { new RsaSecurityKey(rsa) };
                    }
                };

                // Lấy token từ cookie
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Cookies["accessToken"];
                        if (!string.IsNullOrEmpty(accessToken)) context.Token = accessToken;

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();
    }
}