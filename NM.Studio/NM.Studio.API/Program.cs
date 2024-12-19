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
using NM.Studio.Domain.Models;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

#region Add-DbContext

builder.Services.AddDbContext<StudioContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

#endregion

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
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

builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = true;

        // Cấu hình kiểm tra token
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Bật nếu muốn kiểm tra Issuer
            ValidateAudience = false, // Bật nếu muốn kiểm tra Audience
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true, // Bật kiểm tra Signing Key
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetValue<string>("Appsettings:Token"))),
            ClockSkew = TimeSpan.Zero
        };

        // Đọc token từ cookie
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Lấy token từ cookie "accessToken"
                var accessToken = context.Request.Cookies["accessToken"];
                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

#endregion

#region Add-Cors

builder.Services.AddCors(options =>
{
    var frontendDomain = Environment.GetEnvironmentVariable("FRONTEND_DOMAIN");

    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(frontendDomain)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
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

app.UseRouting();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();