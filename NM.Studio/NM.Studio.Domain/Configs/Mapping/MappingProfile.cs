using NM.Studio.Domain.CQRS.Commands.Categories;
using NM.Studio.Domain.CQRS.Commands.Colors;
using NM.Studio.Domain.CQRS.Commands.Sizes;
using NM.Studio.Domain.CQRS.Commands.SubCategories;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Domain.Configs.Mapping;

public partial class MappingProfile
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