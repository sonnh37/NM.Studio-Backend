using NM.Studio.Domain.Contracts.Repositories;

namespace NM.Studio.Domain.Contracts.UnitOfWorks;

public interface IUnitOfWork : IBaseUnitOfWork
{
    IUserRepository UserRepository { get; }
    IServiceRepository ServiceRepository { get; }
    IProductRepository ProductRepository { get; }
    IProductVariantRepository ProductVariantRepository { get; }
    IProductMediaRepository ProductMediaRepository { get; }
    IAlbumRepository AlbumRepository { get; }
    IAlbumImageRepository AlbumImageRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    ISubCategoryRepository SubCategoryRepository { get; }
    IImageRepository ImageRepository { get; }
    IMediaBaseRepository MediaBaseRepository { get; }
    IBlogRepository BlogRepository { get; }
    IServiceBookingRepository ServiceBookingRepository { get; }
    IUserTokenRepository UserTokenRepository { get; }
    IMediaUrlRepository MediaUrlRepository { get; }
    IOrderRepository OrderRepository { get; }
    IOrderItemRepository OrderItemRepository { get; }
    IPaymentRepository PaymentRepository { get; }
    IOrderStatusHistoryRepository OrderStatusHistoryRepository { get; }
    ICartRepository CartRepository { get; }
    ICartItemRepository CartItemRepository { get; }
    IVoucherRepository VoucherRepository { get; }
    IVideoRepository VideoRepository { get; }
    IVoucherUsageHistoryRepository VoucherUsageHistoryRepository { get; }
}