using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class ProductXPhotoResult : BaseResult
{
    public Guid? ProductId { get; set; }

    public Guid? PhotoId { get; set; }

    public ProductResult? Product { get; set; }

    public PhotoResult? Photo { get; set; }
}