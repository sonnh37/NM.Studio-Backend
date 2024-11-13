using AutoMapper;
using NM.Studio.Domain.CQRS.Commands.Categories;
using NM.Studio.Domain.CQRS.Commands.Products;
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

        CreateMap<ProductXPhoto, ProductXPhotoResult>().ReverseMap();
        CreateMap<ProductXPhoto, ProductXPhotoCreateCommand>().ReverseMap();
        CreateMap<ProductXPhoto, ProductXPhotoUpdateCommand>().ReverseMap();
    }
}