using FluentValidation;
using NM.Studio.Domain.Models.CQRS.Commands.ProductVariants;

namespace NM.Studio.Validations.Commands.ProductVariants;

public class ProductVariantCreateCommandValidator : AbstractValidator<ProductVariantCreateCommand>
{
    public ProductVariantCreateCommandValidator()
    {
        RuleFor(request => request.ProductId).NotEmpty();
        RuleFor(request => request.Sku)
            .NotEmpty();
        RuleFor(request => request.Color)
            .NotEmpty();
        RuleFor(request => request.Size)
            .NotEmpty();
        RuleFor(request => request.Status)
            .IsInEnum();
    }
}