using NM.Studio.Domain.Contracts.Repositories;

namespace NM.Studio.Domain.Contracts.UnitOfWorks;

public interface IUnitOfWork : IBaseUnitOfWork
{
    IUserRepository UserRepository { get; }

    IPhotoRepository PhotoRepository { get; }

    IServiceRepository ServiceRepository { get; }

    IProductRepository ProductRepository { get; }

    IProductXPhotoRepository ProductXPhotoRepository { get; }

    IAlbumRepository AlbumRepository { get; }

    IAlbumXPhotoRepository AlbumXPhotoRepository { get; }

    ICategoryRepository CategoryRepository { get; }
    
    ISubCategoryRepository SubCategoryRepository { get; }
    
    IColorRepository ColorRepository { get; }
    
    ISizeRepository SizeRepository { get; }
}