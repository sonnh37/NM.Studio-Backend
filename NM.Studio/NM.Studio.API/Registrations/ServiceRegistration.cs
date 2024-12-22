using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Services;

namespace NM.Studio.API.Registrations;

public static class ServiceRegistration
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<IServiceService, ServiceService>();
        services.AddTransient<IPhotoService, PhotoService>();
        services.AddTransient<IAlbumService, AlbumService>();
        services.AddTransient<IAlbumXPhotoService, AlbumXPhotoService>();
        services.AddTransient<IProductXPhotoService, ProductXPhotoService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<ISizeService, SizeService>();
        services.AddTransient<IColorService, ColorService>();
        services.AddTransient<ISubCategoryService, SubCategoryService>();
        services.AddTransient<IBlogService, BlogService>();
        services.AddTransient<IBookingService, BookingService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IProductXColorService, ProductXColorService>();
        services.AddTransient<IProductXSizeService, ProductXSizeService>();
    }
}