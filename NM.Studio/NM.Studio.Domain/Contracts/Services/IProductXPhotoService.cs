using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.ProductXPhotos;
using NM.Studio.Domain.Models.Responses;

namespace NM.Studio.Domain.Contracts.Services;

public interface IProductXPhotoService : IBaseService
{
    Task<BusinessResult> DeleteById(ProductXPhotoDeleteCommand command);
}