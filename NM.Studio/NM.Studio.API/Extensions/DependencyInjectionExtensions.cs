using NM.Studio.Data.Repositories;
using NM.Studio.Data.UnitOfWorks;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Services;

namespace NM.Studio.API.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<IMediaFileService, MediaFileService>();
        services.AddScoped<IAlbumService, AlbumService>();
        services.AddScoped<IAlbumMediaService, AlbumMediaService>();
        services.AddScoped<IProductMediaService, ProductMediaService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISizeService, SizeService>();
        services.AddScoped<IColorService, ColorService>();
        services.AddScoped<ISubCategoryService, SubCategoryService>();
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IServiceBookingService, ServiceBookingService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IProductColorService, ProductColorService>();
        services.AddScoped<IProductSizeService, ProductSizeService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<ICartItemService, CartItemService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderItemService, OrderItemService>();
        services.AddScoped<IOrderStatusHistoryService, OrderStatusHistoryService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IVoucherService, VoucherService>();
        services.AddScoped<IVoucherUsageHistoryService, VoucherUsageHistoryService>();
    }
    
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IMediaFileRepository, MediaFileRepository>();
        services.AddScoped<IAlbumRepository, AlbumRepository>();
        services.AddScoped<IAlbumMediaRepository, AlbumMediaRepository>();
        services.AddScoped<IProductMediaRepository, ProductMediaRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ISizeRepository, SizeRepository>();
        services.AddScoped<IColorRepository, ColorRepository>();
        services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
        services.AddScoped<IBlogRepository, BlogRepository>();
        services.AddScoped<IServiceBookingRepository, ServiceBookingRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IProductColorRepository, ProductColorRepository>();
        services.AddScoped<IProductSizeRepository, ProductSizeRepository>();
        services.AddScoped<ICartItemRepository, CartItemRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<IOrderStatusHistoryRepository, OrderStatusHistoryRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IVoucherRepository, VoucherRepository>();
        services.AddScoped<IVoucherUsageHistoryRepository, VoucherUsageHistoryRepository>();
    }
    
    
}