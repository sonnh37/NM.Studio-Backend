using AutoMapper;
using NM.Studio.Domain.CQRS.Commands.Products;
using NM.Studio.Domain.CQRS.Commands.Products.Categories;
using NM.Studio.Domain.CQRS.Commands.ProductXPhotos;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Domain.Configs.Mapping;

public partial class MappingProfile : Profile
{
    private void ProductMapping()
    {
        CreateMap<Product, ProductResult>().ReverseMap();
        CreateMap<Product, ProductCreateCommand>().ReverseMap();
        CreateMap<Product, ProductUpdateCommand>().ReverseMap();

        CreateMap<Color, ColorResult>().ReverseMap();
        CreateMap<Size, SizeResult>().ReverseMap();

        CreateMap<Category, CategoryResult>().ReverseMap();
        CreateMap<Category, CategoryCreateCommand>().ReverseMap();
        CreateMap<Category, CategoryUpdateCommand>().ReverseMap();
        
        CreateMap<SubCategory, SubCategoryResult>().ReverseMap();
        // CreateMap<SubCategory, SubCategoryCreateCommand>().ReverseMap();
        // CreateMap<SubCategory, SubCategoryUpdateCommand>().ReverseMap();


        CreateMap<ProductXPhoto, ProductXPhotoResult>().ReverseMap();
        CreateMap<ProductXPhoto, ProductXPhotoCreateCommand>().ReverseMap();
        CreateMap<ProductXPhoto, ProductXPhotoUpdateCommand>().ReverseMap();
    }
}