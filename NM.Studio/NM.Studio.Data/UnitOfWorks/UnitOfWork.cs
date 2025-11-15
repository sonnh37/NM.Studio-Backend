using Microsoft.Extensions.Logging;
using NM.Studio.Data.Context;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.UnitOfWorks;

namespace NM.Studio.Data.UnitOfWorks;

public class UnitOfWork : BaseUnitOfWork<StudioContext>, IUnitOfWork
{
    public UnitOfWork(StudioContext context, IServiceProvider serviceProvider,
        ILogger<BaseUnitOfWork<StudioContext>> logger)
        : base(context, serviceProvider, logger)
    {
    }

    public IUserRepository UserRepository => GetRepository<IUserRepository>();

    public IServiceRepository ServiceRepository => GetRepository<IServiceRepository>();

    public IProductRepository ProductRepository => GetRepository<IProductRepository>();

    public IProductMediaRepository ProductMediaRepository => GetRepository<IProductMediaRepository>();

    public IAlbumRepository AlbumRepository => GetRepository<IAlbumRepository>();

    public IAlbumImageRepository AlbumImageRepository => GetRepository<IAlbumImageRepository>();

    public ICategoryRepository CategoryRepository => GetRepository<ICategoryRepository>();

    public ISubCategoryRepository SubCategoryRepository => GetRepository<ISubCategoryRepository>();

    public IMediaBaseRepository MediaBaseRepository => GetRepository<IMediaBaseRepository>();

    public IBlogRepository BlogRepository => GetRepository<IBlogRepository>();

    public IServiceBookingRepository ServiceBookingRepository => GetRepository<IServiceBookingRepository>();

    public IUserTokenRepository UserTokenRepository => GetRepository<IUserTokenRepository>();

    public IProductVariantRepository ProductVariantRepository => GetRepository<IProductVariantRepository>();

    public IOrderRepository OrderRepository => GetRepository<IOrderRepository>();

    public IOrderItemRepository OrderItemRepository => GetRepository<IOrderItemRepository>();

    public IPaymentRepository PaymentRepository => GetRepository<IPaymentRepository>();

    public IOrderStatusHistoryRepository OrderStatusHistoryRepository => GetRepository<IOrderStatusHistoryRepository>();

    public ICartRepository CartRepository => GetRepository<ICartRepository>();

    public ICartItemRepository CartItemRepository => GetRepository<ICartItemRepository>();

    public IVoucherRepository VoucherRepository => GetRepository<IVoucherRepository>();

    public IVoucherUsageHistoryRepository VoucherUsageHistoryRepository =>
        GetRepository<IVoucherUsageHistoryRepository>();

    public IHomeSlideRepository HomeSlideRepository => GetRepository<IHomeSlideRepository>();
}