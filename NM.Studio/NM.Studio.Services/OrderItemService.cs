using AutoMapper;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Entities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class OrderItemService : BaseService<OrderItem>, IOrderItemService
{
    public OrderItemService(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }
}