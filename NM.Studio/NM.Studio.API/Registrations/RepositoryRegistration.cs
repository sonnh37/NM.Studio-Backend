﻿using NM.Studio.Data.Repositories;
using NM.Studio.Data.UnitOfWorks;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.UnitOfWorks;

namespace NM.Studio.API.Registrations;

public static class RepositoryRegistration
{
    public static void AddCustomRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IPhotoRepository, PhotoRepository>();
        services.AddScoped<IAlbumRepository, AlbumRepository>();
        services.AddScoped<IAlbumXPhotoRepository, AlbumXPhotoRepository>();
        services.AddScoped<IProductXPhotoRepository, ProductXPhotoRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }
}