using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NM.Studio.Data.Context;
using NM.Studio.Data.Repositories;
using NM.Studio.Data.UnitOfWorks;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Models.Options;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services;
using FluentValidation;
using NM.Studio.Validations.Commands.ProductVariants;
using Quartz;

namespace NM.Studio.API.Extensions;

public static class ServiceExtensions
{
    public static readonly string CorsPolicyName = "CorsPolicy";

    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services;
    }

    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            var origins = configuration.GetValue<string>("AllowOrigins")?.Split(",") ?? Array.Empty<string>();
            options.AddPolicy(CorsPolicyName, builder =>
            {
                if (origins.Length == 0)
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                else
                    builder.WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
            });
        });
    }

    public static void ConfigureSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
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
    }

    public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection(nameof(UserJwtOptions)).Get<UserJwtOptions>();

                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RoleClaimType = "Role",
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = jwtOptions!.ValidIssuer,
                    ValidAudience = jwtOptions.ValidAudience,
                    IssuerSigningKey = new RsaSecurityKey(RsaHelper.CreateRsaFromPublicKey(jwtOptions.PublicKey)),
                };
            });

        services.AddAuthorization();
    }

    public static void ConfigureQuartz(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            var jobKey = new JobKey("CleanRefreshTokenJob");
            q.AddJob<CleanRefreshTokenService>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("CleanRefreshTokenJob-trigger")
                .WithCronSchedule("0 0 0 * * ?")); // Cron schedule chạy vào 00:00 mỗi ngày
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }

    public static void ConfigureContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StudioContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            // options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
    }

    public static void ConfigureBind(this IServiceCollection services)
    {
        services.AddOptions<EmailOptions>()
            .BindConfiguration(nameof(EmailOptions));
        services.AddOptions<UserJwtOptions>()
            .BindConfiguration(nameof(UserJwtOptions));
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<IAlbumService, AlbumService>();
        services.AddScoped<IAlbumImageService, AlbumImageService>();
        services.AddScoped<IProductMediaService, ProductMediaService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISubCategoryService, SubCategoryService>();
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IServiceBookingService, ServiceBookingService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IProductVariantService, ProductVariantService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserTokenService, UserTokenService>();
        services.AddScoped<ICartItemService, CartItemService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderItemService, OrderItemService>();
        services.AddScoped<IOrderStatusHistoryService, OrderStatusHistoryService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IVoucherService, VoucherService>();
        services.AddScoped<IVoucherUsageHistoryService, VoucherUsageHistoryService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IMediaUploadService, MediaUploadService>();
        services.AddScoped<IMediaBaseService, MediaBaseService>();
        services.AddScoped<IHomeSlideService, HomeSlideService>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IAlbumRepository, AlbumRepository>();
        services.AddScoped<IAlbumImageRepository, AlbumImageRepository>();
        services.AddScoped<IProductMediaRepository, ProductMediaRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
        services.AddScoped<IBlogRepository, BlogRepository>();
        services.AddScoped<IServiceBookingRepository, ServiceBookingRepository>();
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
        services.AddScoped<ICartItemRepository, CartItemRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<IOrderStatusHistoryRepository, OrderStatusHistoryRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IVoucherRepository, VoucherRepository>();
        services.AddScoped<IVoucherUsageHistoryRepository, VoucherUsageHistoryRepository>();
        services.AddScoped<IMediaBaseRepository, MediaBaseRepository>();
        services.AddScoped<IHomeSlideRepository, HomeSlideRepository>();
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblies(GetAssemblies(), ServiceLifetime.Transient);
        return services;
    }

    public static void LoadAssemblies(this IServiceCollection services)
    {
        var folder = AppContext.BaseDirectory;
        foreach (var dll in Directory.GetFiles(folder, "*.dll"))
        {
            AssemblyLoadContext.Default.LoadFromAssemblyPath(dll);
        }
    }

    private static Assembly[] GetAssemblies()
    {
        return AssemblyLoadContext.Default.Assemblies
            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.FullName))
            .ToArray();
    }
}