using NM.Studio.Data.Context;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.UnitOfWorks;

namespace NM.Studio.Data.UnitOfWorks;

public class UnitOfWork : BaseUnitOfWork<StudioContext>, IUnitOfWork
{
    public UnitOfWork(StudioContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
    }

    public IUserRepository UserRepository => GetRepository<IUserRepository>();

    public IMediaFileRepository MediaFileRepository => GetRepository<IMediaFileRepository>();

    public IServiceRepository ServiceRepository => GetRepository<IServiceRepository>();

    public IProductRepository ProductRepository => GetRepository<IProductRepository>();

    public IProductMediaRepository ProductMediaRepository => GetRepository<IProductMediaRepository>();

    public IAlbumRepository AlbumRepository => GetRepository<IAlbumRepository>();

    public IAlbumMediaRepository AlbumMediaRepository => GetRepository<IAlbumMediaRepository>();

    public ICategoryRepository CategoryRepository => GetRepository<ICategoryRepository>();

    public ISubCategoryRepository SubCategoryRepository => GetRepository<ISubCategoryRepository>();

    public IColorRepository ColorRepository => GetRepository<IColorRepository>();

    public ISizeRepository SizeRepository => GetRepository<ISizeRepository>();

    public IBlogRepository BlogRepository => GetRepository<IBlogRepository>();

    public IServiceBookingRepository ServiceBookingRepository => GetRepository<IServiceBookingRepository>();

    public IRefreshTokenRepository RefreshTokenRepository => GetRepository<IRefreshTokenRepository>();

    public IProductSizeRepository ProductSizeRepository => GetRepository<IProductSizeRepository>();

    public IProductColorRepository ProductColorRepository => GetRepository<IProductColorRepository>();
}