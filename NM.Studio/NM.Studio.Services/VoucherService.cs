using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.Vouchers;
using NM.Studio.Domain.Models.CQRS.Queries.Vouchers;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;
using NM.Studio.Domain.Utilities.Filters;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class VoucherService : BaseService, IVoucherService
{
    private readonly IVoucherRepository _voucherRepository;

    public VoucherService(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
        _voucherRepository = _unitOfWork.VoucherRepository;
    }

    public async Task<BusinessResult> GetAll(VoucherGetAllQuery query)
    {
        var queryable = _voucherRepository.GetQueryable();

        queryable = FilterHelper.BaseEntity(queryable, query);
        queryable = RepoHelper.Include(queryable, query.IncludeProperties);
        queryable = RepoHelper.Sort(queryable, query);

        var totalCount = await queryable.CountAsync();
        var entities = await RepoHelper.GetQueryablePagination(queryable, query).ToListAsync();
        var results = _mapper.Map<List<VoucherResult>>(entities);
        var getQueryableResult = new GetQueryableResult(results, totalCount, query);

        return new BusinessResult(getQueryableResult);
    }

    public async Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand)
    {
        Voucher? entity = null;
        if (createOrUpdateCommand is VoucherUpdateCommand updateCommand)
        {
            entity = await _voucherRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefaultAsync();

            if (entity == null)
                throw new NotFoundException(Const.NOT_FOUND_MSG);

            _mapper.Map(updateCommand, entity);
            _voucherRepository.Update(entity);
        }
        else if (createOrUpdateCommand is VoucherCreateCommand createCommand)
        {
            entity = _mapper.Map<Voucher>(createCommand);
            if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
            entity.CreatedDate = DateTimeOffset.UtcNow;
            _voucherRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<VoucherResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> GetById(VoucherGetByIdQuery request)
    {
        var queryable = _voucherRepository.GetQueryable(x => x.Id == request.Id);
        queryable = RepoHelper.Include(queryable, request.IncludeProperties);
        var entity = await queryable.SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException("Not found");
        var result = _mapper.Map<VoucherResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> Delete(VoucherDeleteCommand command)
    {
        var entity = await _voucherRepository.GetQueryable(x => x.Id == command.Id).SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);

        _voucherRepository.Delete(entity, command.IsPermanent);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        return new BusinessResult();
    }
}