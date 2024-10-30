namespace NM.Studio.Domain.Configs.Mapping;

public partial class MappingProfile
{
    public MappingProfile()
    {
        UserMapping();
        ServiceMapping();
        PhotoMapping();
        ProductMapping();
        AlbumMapping();
    }
}