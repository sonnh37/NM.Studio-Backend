using AutoMapper;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.ProductXPhotos;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class ProductXPhotoService : BaseService<ProductXPhoto>, IProductXPhotoService
{
    private readonly IProductXPhotoRepository _productXPhotoRepository;

    public ProductXPhotoService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _productXPhotoRepository = _unitOfWork.ProductXPhotoRepository;
    }

    public async Task<BusinessResult> DeleteById(ProductXPhotoDeleteCommand command)
    {
        if (command.ProductId == Guid.Empty || command.PhotoId == Guid.Empty)
            return ResponseHelper.Delete(false);

        var entity = await _productXPhotoRepository.GetById(command);
        if (entity == null) return ResponseHelper.Delete(false);
        _productXPhotoRepository.Delete(entity);
        var saveChanges = await _unitOfWork.SaveChanges();

        if (!saveChanges) return ResponseHelper.Delete(false);

        return ResponseHelper.Delete(true);
    }
}