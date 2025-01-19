using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;

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
    
    IBlogRepository BlogRepository { get; }
    
    IBookingRepository BookingRepository { get; }
    
    IRefreshTokenRepository RefreshTokenRepository { get; }
    
    IProductXSizeRepository ProductXSizeRepository { get; }
    
    IProductXColorRepository ProductXColorRepository { get; }
}