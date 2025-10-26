using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.HomeSlides;
using NM.Studio.Domain.Models.CQRS.Queries.HomeSlides;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;
using NM.Studio.Domain.Utilities.Filters;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class HomeSlideService : BaseService, IHomeSlideService
{
    private readonly IHomeSlideRepository _homeSlideRepository;

    public HomeSlideService(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
        _homeSlideRepository = _unitOfWork.HomeSlideRepository;
    }

    public async Task<BusinessResult> GetAll(HomeSlideGetAllQuery query)
    {
        var queryable = _homeSlideRepository.GetQueryable();

        queryable = queryable.FilterBase(query);
        queryable = queryable.Include(query.IncludeProperties);
        queryable = queryable.Sort(query.Sorting);

        var pagedListHomeSlide =
            await queryable.ToPagedListAsync(query.Pagination.PageNumber, query.Pagination.PageSize);
        var pagedList = _mapper.Map<IPagedList<HomeSlideResult>>(pagedListHomeSlide);

        return new BusinessResult(pagedList);
    }


    public async Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand)
    {
        HomeSlide? entity = null;
        if (createOrUpdateCommand is HomeSlideUpdateCommand updateCommand)
        {
            entity = await _homeSlideRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefaultAsync();

            if (entity == null)
                throw new NotFoundException(Const.NOT_FOUND_MSG);

            if (entity.DisplayOrder == updateCommand.DisplayOrder)
                throw new DomainException("Display order must be different");

            _mapper.Map(updateCommand, entity);
            _homeSlideRepository.Update(entity);
        }
        else if (createOrUpdateCommand is HomeSlideCreateCommand createCommand)
        {
            var queryable = _homeSlideRepository.GetQueryable();
            var q = await queryable.Where(n => n.DisplayOrder == createCommand.DisplayOrder).FirstOrDefaultAsync();
            if (q != null) throw new DomainException($"Display order `{q.DisplayOrder}` had been used");

            entity = _mapper.Map<HomeSlide>(createCommand);
            if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
            entity.CreatedDate = DateTimeOffset.UtcNow;
            _homeSlideRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<HomeSlideResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> GetById(HomeSlideGetByIdQuery request)
    {
        var queryable = _homeSlideRepository.GetQueryable(x => x.Id == request.Id);
        queryable = RepoHelper.Include(queryable, request.IncludeProperties);
        var entity = await queryable.SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException("Not found");
        var result = _mapper.Map<HomeSlideResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> Delete(HomeSlideDeleteCommand command)
    {
        var entity = await _homeSlideRepository.GetQueryable(x => x.Id == command.Id).SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);

        _homeSlideRepository.Delete(entity, command.IsPermanent);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        return new BusinessResult();
    }
}