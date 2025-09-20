using AutoMapper;
using NM.Studio.Domain.CQRS.Commands.AlbumImages;
using NM.Studio.Domain.CQRS.Commands.Albums;
using NM.Studio.Domain.CQRS.Commands.Blogs;
using NM.Studio.Domain.CQRS.Commands.CartItems;
using NM.Studio.Domain.CQRS.Commands.Carts;
using NM.Studio.Domain.CQRS.Commands.Categories;
using NM.Studio.Domain.CQRS.Commands.Images;
using NM.Studio.Domain.CQRS.Commands.OrderItems;
using NM.Studio.Domain.CQRS.Commands.Orders;
using NM.Studio.Domain.CQRS.Commands.OrderStatusHistories;
using NM.Studio.Domain.CQRS.Commands.Payments;
using NM.Studio.Domain.CQRS.Commands.ProductImages;
using NM.Studio.Domain.CQRS.Commands.Products;
using NM.Studio.Domain.CQRS.Commands.ProductVariants;
using NM.Studio.Domain.CQRS.Commands.ServiceBookings;
using NM.Studio.Domain.CQRS.Commands.Services;
using NM.Studio.Domain.CQRS.Commands.SubCategories;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Commands.UserTokens;
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
        ImageMapping();
        ProductMapping();
        AlbumMapping();
        MediaBaseMapping();
        MediaUrlMapping();
        VideoMapping();
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

        CreateMap<AlbumImage, AlbumImageResult>().ReverseMap();
        CreateMap<AlbumImage, AlbumImageCreateCommand>().ReverseMap();
        CreateMap<AlbumImage, AlbumImageUpdateCommand>().ReverseMap();
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

        CreateMap<ProductVariant, ProductVariantResult>().ReverseMap();
        CreateMap<ProductVariant, ProductVariantCreateCommand>().ReverseMap();
        CreateMap<ProductVariant, ProductVariantUpdateCommand>().ReverseMap();
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
        CreateMap<UserToken, UserTokenResult>().ReverseMap();
        CreateMap<UserToken, UserTokenCreateCommand>().ReverseMap();
        CreateMap<UserToken, UserTokenUpdateCommand>().ReverseMap();
    }

    private void ImageMapping()
    {
        CreateMap<Image, ImageResult>().ReverseMap();
        CreateMap<Image, ImageCreateCommand>().ReverseMap();
        CreateMap<Image, ImageUpdateCommand>().ReverseMap();
    }

    private void MediaBaseMapping()
    {
        CreateMap<MediaBase, MediaBaseResult>().ReverseMap();
        // CreateMap<MediaBase, MediaBaseCreateCommand>().ReverseMap();
        // CreateMap<MediaBase, MediaBaseUpdateCommand>().ReverseMap();
    }

    private void MediaUrlMapping()
    {
        CreateMap<MediaUrl, MediaUrlResult>().ReverseMap();
    }

    private void VideoMapping()
    {
        CreateMap<Video, VideoResult>().ReverseMap();
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