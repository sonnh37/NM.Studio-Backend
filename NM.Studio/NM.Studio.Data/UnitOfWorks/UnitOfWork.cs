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
    public IOrderRepository OrderRepository => GetRepository<IOrderRepository>();
    public IOrderItemRepository OrderItemRepository => GetRepository<IOrderItemRepository>();
    public IPaymentRepository PaymentRepository => GetRepository<IPaymentRepository>();
    public IOrderStatusHistoryRepository OrderStatusHistoryRepository => GetRepository<IOrderStatusHistoryRepository>();
    public ICartRepository CartRepository => GetRepository<ICartRepository>();
    public ICartItemRepository CartItemRepository => GetRepository<ICartItemRepository>();
    public IVoucherRepository VoucherRepository => GetRepository<IVoucherRepository>();
    public IVoucherUsageHistoryRepository VoucherUsageHistoryRepository => GetRepository<IVoucherUsageHistoryRepository>();
}