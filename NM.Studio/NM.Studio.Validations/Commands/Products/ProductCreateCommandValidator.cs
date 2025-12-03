using FluentValidation;
using NM.Studio.Domain.Models.CQRS.Commands.Products;

namespace NM.Studio.Validations.Commands.Products;

public class ProductCreateCommandValidator : AbstractValidator<ProductCreateCommand>
{
    public ProductCreateCommandValidator()
    {
        RuleFor(request => request.Name).NotEmpty();
        RuleFor(request => request.Sku)
            .NotEmpty();
        RuleFor(request => request.CategoryId)
            .NotEmpty();
        RuleFor(request => request.SubCategoryId)
            .NotEmpty();
        RuleFor(request => request.Status)
            .NotEmpty();
    }
}