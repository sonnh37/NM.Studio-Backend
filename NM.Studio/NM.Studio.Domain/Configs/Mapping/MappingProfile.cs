using AutoMapper;
using NM.Studio.Domain.CQRS.Commands.Albums;
using NM.Studio.Domain.CQRS.Commands.AlbumMedias;
using NM.Studio.Domain.CQRS.Commands.Blogs;
using NM.Studio.Domain.CQRS.Commands.CartItems;
using NM.Studio.Domain.CQRS.Commands.Carts;
using NM.Studio.Domain.CQRS.Commands.Categories;
using NM.Studio.Domain.CQRS.Commands.Colors;
using NM.Studio.Domain.CQRS.Commands.MediaFiles;
using NM.Studio.Domain.CQRS.Commands.OrderItems;
using NM.Studio.Domain.CQRS.Commands.Orders;
using NM.Studio.Domain.CQRS.Commands.OrderStatusHistories;
using NM.Studio.Domain.CQRS.Commands.Payments;
using NM.Studio.Domain.CQRS.Commands.Products;
using NM.Studio.Domain.CQRS.Commands.ProductColors;
using NM.Studio.Domain.CQRS.Commands.ProductMedias;
using NM.Studio.Domain.CQRS.Commands.ProductSizes;
using NM.Studio.Domain.CQRS.Commands.RefreshTokens;
using NM.Studio.Domain.CQRS.Commands.ServiceBookings;
using NM.Studio.Domain.CQRS.Commands.Services;
using NM.Studio.Domain.CQRS.Commands.Sizes;
using NM.Studio.Domain.CQRS.Commands.SubCategories;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Commands.Vouchers;
using NM.Studio.Domain.CQRS.Commands.VoucherUsageHistories;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Domain.Configs.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CategoryMapping();
        SubCategoryMapping();
        UserMapping();
        ServiceMapping();
        MediaFileMapping();
        ProductMapping();
        AlbumMapping();
        SizeMapping();
        ColorMapping();
        BlogMapping();
        BookingMapping();
        CartMapping();
        CartItemMapping();
        OrderMapping();
        OrderItemMapping();
        OrderStatusHistoryMapping();
        PaymentMapping();
        VoucherMapping();
        VoucherUsageHistoryMapping();
    }

    private void AlbumMapping()
    {
        CreateMap<Album, AlbumResult>().ReverseMap();
        CreateMap<Album, AlbumCreateCommand>().ReverseMap();
        CreateMap<Album, AlbumUpdateCommand>().ReverseMap();

        CreateMap<AlbumMedia, AlbumMediaResult>().ReverseMap();
        CreateMap<AlbumMedia, AlbumMediaCreateCommand>().ReverseMap();
        CreateMap<AlbumMedia, AlbumMediaUpdateCommand>().ReverseMap();
    }

    private void BookingMapping()
    {
        CreateMap<ServiceBooking, ServiceBookingResult>().ReverseMap();
        CreateMap<ServiceBooking, ServiceBookingCreateCommand>().ReverseMap();
        CreateMap<ServiceBooking, ServiceBookingUpdateCommand>().ReverseMap();
    }

    private void ProductMapping()
    {
        CreateMap<Product, ProductResult>().ReverseMap();
        CreateMap<Product, ProductCreateCommand>().ReverseMap();
        CreateMap<Product, ProductUpdateCommand>().ReverseMap();

        CreateMap<ProductMedia, ProductMediaResult>().ReverseMap();
        CreateMap<ProductMedia, ProductMediaCreateCommand>().ReverseMap();
        CreateMap<ProductMedia, ProductMediaUpdateCommand>().ReverseMap();

        CreateMap<ProductColor, ProductColorResult>().ReverseMap();
        CreateMap<ProductColor, ProductColorCreateCommand>().ReverseMap();
        CreateMap<ProductColor, ProductColorUpdateCommand>().ReverseMap();

        CreateMap<ProductSize, ProductSizeResult>().ReverseMap();
        CreateMap<ProductSize, ProductSizeCreateCommand>().ReverseMap();
        CreateMap<ProductSize, ProductSizeUpdateCommand>().ReverseMap();
    }

    private void ServiceMapping()
    {
        CreateMap<Service, ServiceResult>().ReverseMap();
        CreateMap<Service, ServiceCreateCommand>().ReverseMap();
        CreateMap<Service, ServiceUpdateCommand>().ReverseMap();
    }

    private void BlogMapping()
    {
        CreateMap<Blog, BlogResult>().ReverseMap();
        CreateMap<Blog, BlogCreateCommand>().ReverseMap();
        CreateMap<Blog, BlogUpdateCommand>().ReverseMap();
    }
    
    private void CartMapping()
    {
        CreateMap<Cart, CartResult>().ReverseMap();
        CreateMap<Cart, CartCreateCommand>().ReverseMap();
        CreateMap<Cart, CartUpdateCommand>().ReverseMap();
    }
    
    private void CartItemMapping()
    {
        CreateMap<CartItem, CartItemResult>().ReverseMap();
        CreateMap<CartItem, CartItemCreateCommand>().ReverseMap();
        CreateMap<CartItem, CartItemUpdateCommand>().ReverseMap();
    }
    
    private void OrderMapping()
    {
        CreateMap<Order, OrderResult>().ReverseMap();
        CreateMap<Order, OrderCreateCommand>().ReverseMap();
        CreateMap<Order, OrderUpdateCommand>().ReverseMap();
    }
    
    private void OrderItemMapping()
    {
        CreateMap<OrderItem, OrderItemResult>().ReverseMap();
        CreateMap<OrderItem, OrderItemCreateCommand>().ReverseMap();
        CreateMap<OrderItem, OrderItemUpdateCommand>().ReverseMap();
    }
    
    private void OrderStatusHistoryMapping()
    {
        CreateMap<OrderStatusHistory, OrderStatusHistoryResult>().ReverseMap();
        CreateMap<OrderStatusHistory, OrderStatusHistoryCreateCommand>().ReverseMap();
        CreateMap<OrderStatusHistory, OrderStatusHistoryUpdateCommand>().ReverseMap();
    }
    
    private void PaymentMapping()
    {
        CreateMap<Payment, PaymentResult>().ReverseMap();
        CreateMap<Payment, PaymentCreateCommand>().ReverseMap();
        CreateMap<Payment, PaymentUpdateCommand>().ReverseMap();
    }
    
    private void VoucherMapping()
    {
        CreateMap<Voucher, VoucherResult>().ReverseMap();
        CreateMap<Voucher, VoucherCreateCommand>().ReverseMap();
        CreateMap<Voucher, VoucherUpdateCommand>().ReverseMap();
    }
    
    private void VoucherUsageHistoryMapping()
    {
        CreateMap<VoucherUsageHistory, VoucherUsageHistoryResult>().ReverseMap();
        CreateMap<VoucherUsageHistory, VoucherUsageHistoryCreateCommand>().ReverseMap();
        CreateMap<VoucherUsageHistory, VoucherUsageHistoryUpdateCommand>().ReverseMap();
    }

    private void UserMapping()
    {
        CreateMap<User, UserResult>().ReverseMap();
        CreateMap<User, UserCreateCommand>().ReverseMap();
        CreateMap<User, UserUpdateCommand>().ReverseMap();
        CreateMap<RefreshToken, RefreshTokenResult>().ReverseMap();
        CreateMap<RefreshToken, RefreshTokenCreateCommand>().ReverseMap();
        CreateMap<RefreshToken, RefreshTokenUpdateCommand>().ReverseMap();
    }

    private void MediaFileMapping()
    {
        CreateMap<MediaFile, MediaFileResult>().ReverseMap();
        CreateMap<MediaFile, MediaFileCreateCommand>().ReverseMap();
        CreateMap<MediaFile, MediaFileUpdateCommand>().ReverseMap();
        CreateMap<MediaFileResult, MediaFileUpdateCommand>().ReverseMap();
    }

    private void SizeMapping()
    {
        CreateMap<Size, SizeResult>().ReverseMap();
        CreateMap<Size, SizeCreateCommand>().ReverseMap();
        CreateMap<Size, SizeUpdateCommand>().ReverseMap();
    }

    private void ColorMapping()
    {
        CreateMap<Color, ColorResult>().ReverseMap();
        CreateMap<Color, ColorCreateCommand>().ReverseMap();
        CreateMap<Color, ColorUpdateCommand>().ReverseMap();
    }

    private void CategoryMapping()
    {
        CreateMap<Category, CategoryResult>().ReverseMap();
        CreateMap<Category, CategoryCreateCommand>().ReverseMap();
        CreateMap<Category, CategoryUpdateCommand>().ReverseMap();
    }

    private void SubCategoryMapping()
    {
        CreateMap<SubCategory, SubCategoryResult>().ReverseMap();
        CreateMap<SubCategory, SubCategoryCreateCommand>().ReverseMap();
        CreateMap<SubCategory, SubCategoryUpdateCommand>().ReverseMap();
    }
}