using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class ProductMediaResult : BaseResult
{
    public Guid? ProductId { get; set; }

    public Guid? MediaFileId { get; set; }

    public ProductResult? Product { get; set; }

    public MediaFileResult? MediaFile { get; set; }
}