using NM.Studio.Domain.Contracts.Repositories;

namespace NM.Studio.Domain.Contracts.UnitOfWorks;

public interface IUnitOfWork : IBaseUnitOfWork
{
    IUserRepository UserRepository { get; }

    IMediaFileRepository MediaFileRepository { get; }

    IServiceRepository ServiceRepository { get; }

    IProductRepository ProductRepository { get; }

    IProductMediaRepository ProductMediaRepository { get; }

    IAlbumRepository AlbumRepository { get; }

    IAlbumMediaRepository AlbumMediaRepository { get; }

    ICategoryRepository CategoryRepository { get; }

    ISubCategoryRepository SubCategoryRepository { get; }

    IColorRepository ColorRepository { get; }

    ISizeRepository SizeRepository { get; }

    IBlogRepository BlogRepository { get; }

    IServiceBookingRepository ServiceBookingRepository { get; }

    IRefreshTokenRepository RefreshTokenRepository { get; }

    IProductSizeRepository ProductSizeRepository { get; }

    IProductColorRepository ProductColorRepository { get; }
    
    IOrderRepository OrderRepository { get; }
    
    IOrderItemRepository OrderItemRepository { get; }
    
    IPaymentRepository PaymentRepository { get; }
    
    IOrderStatusHistoryRepository OrderStatusHistoryRepository { get; }
    
    ICartRepository CartRepository { get; }
    
    ICartItemRepository CartItemRepository { get; }
    
    IVoucherRepository VoucherRepository { get; }
    
    IVoucherUsageHistoryRepository VoucherUsageHistoryRepository { get; }
}