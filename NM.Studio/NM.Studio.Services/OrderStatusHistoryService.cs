using AutoMapper;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class OrderStatusHistoryService : BaseService, IOrderStatusHistoryService
{
    private readonly IOrderStatusHistoryRepository _orderStatusHistoryRepository;

    public OrderStatusHistoryService(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
        _orderStatusHistoryRepository = _unitOfWork.OrderStatusHistoryRepository;
    }
}