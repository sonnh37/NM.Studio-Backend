using AutoMapper;
using NM.Studio.Domain.CQRS.Commands.Albums;
using NM.Studio.Domain.CQRS.Commands.AlbumXPhotos;
using NM.Studio.Domain.CQRS.Commands.Blogs;
using NM.Studio.Domain.CQRS.Commands.Bookings;
using NM.Studio.Domain.CQRS.Commands.Categories;
using NM.Studio.Domain.CQRS.Commands.Colors;
using NM.Studio.Domain.CQRS.Commands.Photos;
using NM.Studio.Domain.CQRS.Commands.Products;
using NM.Studio.Domain.CQRS.Commands.ProductXColors;
using NM.Studio.Domain.CQRS.Commands.ProductXPhotos;
using NM.Studio.Domain.CQRS.Commands.ProductXSizes;
using NM.Studio.Domain.CQRS.Commands.Services;
using NM.Studio.Domain.CQRS.Commands.Sizes;
using NM.Studio.Domain.CQRS.Commands.SubCategories;
using NM.Studio.Domain.CQRS.Commands.Users;
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
        PhotoMapping();
        ProductMapping();
        AlbumMapping();
        SizeMapping();
        ColorMapping();
        BlogMapping();
        BookingMapping();
    }

    private void AlbumMapping()
    {
        CreateMap<Album, AlbumResult>().ReverseMap();
        CreateMap<Album, AlbumCreateCommand>().ReverseMap();
        CreateMap<Album, AlbumUpdateCommand>().ReverseMap();

        CreateMap<AlbumXPhoto, AlbumXPhotoResult>().ReverseMap();
        CreateMap<AlbumXPhoto, AlbumXPhotoCreateCommand>().ReverseMap();
        CreateMap<AlbumXPhoto, AlbumXPhotoUpdateCommand>().ReverseMap();
    }

    private void BookingMapping()
    {
        CreateMap<Booking, BookingResult>().ReverseMap();
        CreateMap<Booking, BookingCreateCommand>().ReverseMap();
        CreateMap<Booking, BookingUpdateCommand>().ReverseMap();
    }

    private void ProductMapping()
    {
        CreateMap<Product, ProductResult>().ReverseMap();
        CreateMap<Product, ProductCreateCommand>().ReverseMap();
        CreateMap<Product, ProductUpdateCommand>().ReverseMap();

        CreateMap<ProductXPhoto, ProductXPhotoResult>().ReverseMap();
        CreateMap<ProductXPhoto, ProductXPhotoCreateCommand>().ReverseMap();
        CreateMap<ProductXPhoto, ProductXPhotoUpdateCommand>().ReverseMap();

        CreateMap<ProductXColor, ProductXColorResult>().ReverseMap();
        CreateMap<ProductXColor, ProductXColorCreateCommand>().ReverseMap();
        CreateMap<ProductXColor, ProductXColorUpdateCommand>().ReverseMap();

        CreateMap<ProductXSize, ProductXSizeResult>().ReverseMap();
        CreateMap<ProductXSize, ProductXSizeCreateCommand>().ReverseMap();
        CreateMap<ProductXSize, ProductXSizeUpdateCommand>().ReverseMap();
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

    private void UserMapping()
    {
        CreateMap<User, UserResult>().ReverseMap();
        CreateMap<User, UserCreateCommand>().ReverseMap();
        CreateMap<User, UserUpdateCommand>().ReverseMap();
        CreateMap<UserRefreshToken, UserRefreshTokenResult>().ReverseMap();
    }

    private void PhotoMapping()
    {
        CreateMap<Photo, PhotoResult>().ReverseMap();
        CreateMap<Photo, PhotoCreateCommand>().ReverseMap();
        CreateMap<Photo, PhotoUpdateCommand>().ReverseMap();
        CreateMap<PhotoResult, PhotoUpdateCommand>().ReverseMap();
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