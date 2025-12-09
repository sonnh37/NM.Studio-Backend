using FluentValidation;
using NM.Studio.Domain.Models.CQRS.Commands.AlbumImages;

namespace NM.Studio.Validations.Commands.AlbumImages;

public class AlbumImageCreateCommandValidator : AbstractValidator<AlbumImageCreateCommand>
{
    public AlbumImageCreateCommandValidator()
    {
        RuleFor(request => request.AlbumId).NotEmpty();
        RuleFor(request => request.ImageId).NotEmpty();
    }
}