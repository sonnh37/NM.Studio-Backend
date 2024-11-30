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

    public IPhotoRepository PhotoRepository => GetRepository<IPhotoRepository>();

    public IServiceRepository ServiceRepository => GetRepository<IServiceRepository>();

    public IProductRepository ProductRepository => GetRepository<IProductRepository>();

    public IProductXPhotoRepository ProductXPhotoRepository => GetRepository<IProductXPhotoRepository>();

    public IAlbumRepository AlbumRepository => GetRepository<IAlbumRepository>();

    public IAlbumXPhotoRepository AlbumXPhotoRepository => GetRepository<IAlbumXPhotoRepository>();

    public ICategoryRepository CategoryRepository => GetRepository<ICategoryRepository>();
    
    public ISubCategoryRepository SubCategoryRepository => GetRepository<ISubCategoryRepository>();
    
    public IColorRepository ColorRepository => GetRepository<IColorRepository>();
    
    public ISizeRepository SizeRepository => GetRepository<ISizeRepository>();
    
    public IBlogRepository BlogRepository => GetRepository<IBlogRepository>();
}