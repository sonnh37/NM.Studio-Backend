using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.CQRS.Commands.Services;
using NM.Studio.Domain.CQRS.Queries.Services;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;
using NM.Studio.Domain.Utilities.Filters;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class ServiceService : BaseService, IServiceService
{
    private readonly IServiceRepository _serviceRepository;

    public ServiceService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _serviceRepository = _unitOfWork.ServiceRepository;
    }

    public async Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand)
    {
        Service? entity = null;
        if (createOrUpdateCommand is ServiceUpdateCommand updateCommand)
        {
            updateCommand.Slug = SlugHelper.ToSlug(updateCommand.Name);
            entity = _serviceRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefault();

            if (entity == null) throw new NotFoundException("NotFound");

            // check if update input slug != current slug
            if (updateCommand.Slug != entity.Slug)
            {
                // continue check if input slug == any slug
                var isExistSlug = await _serviceRepository.GetQueryable(m => m.Slug == updateCommand.Slug).AnyAsync();

                if (isExistSlug) throw new DomainException("The service's name already exists");
            }

            _mapper.Map(updateCommand, entity);
            _serviceRepository.Update(entity);
        }
        else if (createOrUpdateCommand is ServiceCreateCommand createCommand)
        {
            createCommand.Slug = SlugHelper.ToSlug(createCommand.Name);
            var isExistSlug = await _serviceRepository.GetQueryable(m => m.Slug == createCommand.Slug).AnyAsync();
            if (isExistSlug) throw new DomainException("The service's name already exists");

            entity = _mapper.Map<Service>(createCommand);
            if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
            entity.CreatedDate = DateTimeOffset.UtcNow;
            _serviceRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<ServiceResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> GetAll(ServiceGetAllQuery query)
    {
        var queryable = _serviceRepository.GetQueryable();

        queryable = FilterHelper.BaseEntity(queryable, query);
        queryable = RepoHelper.Include(queryable, query.IncludeProperties);
        queryable = RepoHelper.Sort(queryable, query);

        var totalCount = await queryable.CountAsync();
        var entities = await RepoHelper.GetQueryablePagination(queryable, query).ToListAsync();
        var results = _mapper.Map<List<ServiceResult>>(entities);
        var getQueryableResult = new GetQueryableResult(results, totalCount, query);

        return new BusinessResult(getQueryableResult);
    }

    public async Task<BusinessResult> GetById(ServiceGetByIdQuery request)
    {
        var queryable = _serviceRepository.GetQueryable(x => x.Id == request.Id);
        queryable = RepoHelper.Include(queryable, request.IncludeProperties);
        var entity = await queryable.SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException("Not found");
        var result = _mapper.Map<ServiceResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> Delete(ServiceDeleteCommand command)
    {
        var entity = await _serviceRepository.GetQueryable(x => x.Id == command.Id).SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);

        _serviceRepository.Delete(entity, command.IsPermanent);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        return new BusinessResult();
    }
}