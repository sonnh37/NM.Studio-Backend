using NM.Studio.Domain.Contracts.Services.Bases;
using NM.Studio.Domain.Models.CQRS.Commands.Base;
using NM.Studio.Domain.Models.CQRS.Commands.Vouchers;
using NM.Studio.Domain.Models.CQRS.Queries.Vouchers;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IVoucherService : IBaseService
{
    Task<BusinessResult> GetAll(VoucherGetAllQuery query);
    Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand);
    Task<BusinessResult> GetById(VoucherGetByIdQuery request);
    Task<BusinessResult> Delete(VoucherDeleteCommand command);
}