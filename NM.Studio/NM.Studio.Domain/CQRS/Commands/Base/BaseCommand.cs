using System.ComponentModel;
using MediatR;
using NM.Studio.Domain.Models.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace NM.Studio.Domain.CQRS.Commands.Base;

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
    [DefaultValue(typeof(Guid), "00000000-0000-0000-0000-000000000000")]
    public Guid Id { get; set; }

    [DefaultValue(null)]
    public bool? IsDeleted { get; set; }
}

public class DeleteCommand : BaseCommand, IRequest<BusinessResult>
{
    public Guid Id { get; set; }
    
    public bool IsPermanent { get; set; }
}