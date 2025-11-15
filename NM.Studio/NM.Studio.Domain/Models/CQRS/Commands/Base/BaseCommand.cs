using MediatR;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.CQRS.Commands.Base;

public abstract class BaseCommand
{
}

public class CreateOrUpdateCommand : BaseCommand, IRequest<BusinessResult>
{
}

public class CreateCommand : CreateOrUpdateCommand
{
}

public class UpdateCommand : CreateOrUpdateCommand
{
    public Guid Id { get; set; }

    public bool IsDeleted { get; set; } = false;
}

public class DeleteCommand : BaseCommand, IRequest<BusinessResult>
{
    public Guid Id { get; set; }

    public bool IsPermanent { get; set; } = false;
}