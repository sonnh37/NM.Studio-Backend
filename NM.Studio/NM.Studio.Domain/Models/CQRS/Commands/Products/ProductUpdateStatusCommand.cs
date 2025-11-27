using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.Products;

public class ProductUpdateStatusCommand : UpdateCommand
{
    public ProductStatus Status { get; set; }
}