using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.SubCategories;
using NM.Studio.Domain.Models.CQRS.Queries.SubCategories;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;
using NM.Studio.Domain.Utilities.Filters;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class SubCategoryService : BaseService, ISubCategoryService
{
    private readonly ISubCategoryRepository _subCategoryRepository;

    public SubCategoryService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _subCategoryRepository = _unitOfWork.SubCategoryRepository;
    }

    public async Task<BusinessResult> GetAll(SubCategoryGetAllQuery query)
    {
        var queryable = _subCategoryRepository.GetQueryable();

        queryable = queryable.FilterBase(query);
        queryable = queryable.Include(query.IncludeProperties);
        queryable = queryable.Sort(query.Sorting);

        var pagedListSubCategory = await queryable.ToPagedListAsync(query.Pagination.PageNumber, query.Pagination.PageSize);
        var pagedList = _mapper.Map<IPagedList<SubCategoryResult>>(pagedListSubCategory);

        return new BusinessResult(pagedList);
    }

    public async Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand)
    {
        SubCategory? entity = null;
        if (createOrUpdateCommand is SubCategoryUpdateCommand updateCommand)
        {
            entity = await _subCategoryRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefaultAsync();

            if (entity == null)
                throw new NotFoundException(Const.NOT_FOUND_MSG);

            _mapper.Map(updateCommand, entity);
            _subCategoryRepository.Update(entity);
        }
        else if (createOrUpdateCommand is SubCategoryCreateCommand createCommand)
        {
            entity = _mapper.Map<SubCategory>(createCommand);
            if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
            entity.CreatedDate = DateTimeOffset.UtcNow;
            _subCategoryRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<SubCategoryResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> GetById(SubCategoryGetByIdQuery request)
    {
        var queryable = _subCategoryRepository.GetQueryable(x => x.Id == request.Id);
        queryable = RepoHelper.Include(queryable, request.IncludeProperties);
        var entity = await queryable.SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException("Not found");
        var result = _mapper.Map<SubCategoryResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> Delete(SubCategoryDeleteCommand command)
    {
        var entity = await _subCategoryRepository.GetQueryable(x => x.Id == command.Id).SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);

        _subCategoryRepository.Delete(entity, command.IsPermanent);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        return new BusinessResult();
    }
}