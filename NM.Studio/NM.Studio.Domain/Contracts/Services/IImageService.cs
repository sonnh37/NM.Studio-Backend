using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.CQRS.Commands.Images;
using NM.Studio.Domain.CQRS.Queries.Images;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IImageService : IBaseService
{
    Task<BusinessResult> GetAll(ImageGetAllQuery query);
    Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand);
    Task<BusinessResult> GetById(ImageGetByIdQuery request);
    Task<BusinessResult> Delete(ImageDeleteCommand command);
}