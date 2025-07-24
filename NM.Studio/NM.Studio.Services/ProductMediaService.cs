using AutoMapper;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.ProductMedias;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class ProductMediaService : BaseService<ProductMedia>, IProductMediaService
{
    private readonly IProductMediaRepository _productMediaRepository;

    public ProductMediaService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _productMediaRepository = _unitOfWork.ProductMediaRepository;
    }

    public async Task<BusinessResult> DeleteById(ProductMediaDeleteCommand command)
    {
        if (command.ProductId == Guid.Empty || command.MediaFileId == Guid.Empty)
            return BusinessResult.Fail(Const.NOT_FOUND_MSG);


        var entity = await _productMediaRepository.GetById(command);
        if (entity == null)
            return BusinessResult.Fail(Const.NOT_FOUND_MSG);

        _productMediaRepository.Delete(entity);
        var saveChanges = await _unitOfWork.SaveChanges();

        if (!saveChanges)
            return BusinessResult.Fail(Const.FAIL_SAVE_MSG);


        return BusinessResult.Success();
    }
}