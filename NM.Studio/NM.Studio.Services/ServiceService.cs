using AutoMapper;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Services;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class ServiceService : BaseService<Service>, IServiceService
{
    private readonly IServiceRepository _serviceRepository;

    public ServiceService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _serviceRepository = _unitOfWork.ServiceRepository;
    }

    public async Task<BusinessResult> Create<TResult>(ServiceCreateCommand createCommand) where TResult : BaseResult
    {
        try
        {
            createCommand.Slug = SlugHelper.ToSlug(createCommand.Name);
            var service = _serviceRepository.GetQueryable(m => m.Slug == createCommand.Slug).SingleOrDefault();
            if (service != null) return BusinessResult.Fail("The service's name already exists");

            var entity = await CreateOrUpdateEntity(createCommand);
            var result = _mapper.Map<TResult>(entity);

            if (result == null) return BusinessResult.Fail(Const.FAIL_SAVE_MSG);
            return BusinessResult.Success(result);
        }
        catch (Exception ex)
        {
            return BusinessResult.ExceptionError(ex.Message);
        }
    }

    public async Task<BusinessResult> Update<TResult>(ServiceUpdateCommand updateCommand) where TResult : BaseResult
    {
        try
        {
            updateCommand.Slug = SlugHelper.ToSlug(updateCommand.Name);
            var service = _serviceRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefault();

            if (service == null) return BusinessResult.Fail("NotFound");

            // check if update input slug != current slug
            if (updateCommand.Slug != service?.Slug)
            {
                // continue check if input slug == any slug
                var service_ = _serviceRepository.GetQueryable(m => m.Slug == updateCommand.Slug).SingleOrDefault();

                if (service_ != null) return BusinessResult.Fail("The service's name already exists");
            }

            var entity = await CreateOrUpdateEntity(updateCommand);
            var result = _mapper.Map<TResult>(entity);

            if (result == null) return BusinessResult.Fail(Const.FAIL_SAVE_MSG);
            return BusinessResult.Success(result);
        }
        catch (Exception ex)
        {
            return BusinessResult.ExceptionError(ex.Message);
        }
    }
}