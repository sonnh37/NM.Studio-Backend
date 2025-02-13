﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using NM.Studio.API.Registrations;
using NM.Studio.Data;
using NM.Studio.Data.Context;
using NM.Studio.Domain.Configs.Mapping;
using NM.Studio.Domain.Middleware;
using NM.Studio.Handler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models;
using NM.Studio.Services;
using Quartz;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging();
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var jobKey = new JobKey("CleanRefreshTokenJob");
    q.AddJob<CleanRefreshTokenJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("CleanRefreshTokenJob-trigger")
        .WithCronSchedule("0 0 0 * * ?")); // Cron schedule chạy vào 00:00 mỗi ngày
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

#region Add-DbContext

builder.Services.AddDbContext<StudioContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

#endregion

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    // options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(MappingProfile));

#region Add-MediaR

//After 12.0.0
//builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(AppHandler).GetTypeInfo().Assembly));

builder.Services.AddApplication();
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


// var handler = typeof(AppHandler).GetTypeInfo().Assembly;
// builder.Services.AddMediatR(Assembly.GetExecutingAssembly(), handler);

#endregion

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddCustomRepositories();
builder.Services.AddCustomServices();

#region Config-Authentication_Authorization

builder.Services.Configure<TokenSetting>(builder.Configuration.GetSection("TokenSetting"));

builder.Services.AddAuthentication(x =>
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
                    builder.Services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>();

                var authService = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IAuthService>();
                if (authService == null)
                {
                    throw new SecurityTokenException("AuthService not available.");
                }
                
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
                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            },
        };
    });

builder.Services.AddAuthorization();

#endregion

#region Add-Cors

builder.Services.AddCors(options =>
{
    var frontendDomains = Environment.GetEnvironmentVariable("FRONTEND_DOMAIN")?.Split(',');

    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins(frontendDomains) // Thêm các domain của frontend
            .AllowCredentials() // Cho phép gửi cookie
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

#endregion


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -----------------app-------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<AuthenticationMiddleware>();

// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetRequiredService<StudioContext>();
//     DummyData.SeedDatabase(context);
// }

app.UseHttpsRedirection();
app.UseRouting();


app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();