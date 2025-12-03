using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.Base;

namespace NM.Studio.Domain.Models.CQRS.Commands.ProductVariants;

public class ProductVariantUpdateStatusCommand : UpdateCommand
{
    public InventoryStatus Status { get; set; }
}