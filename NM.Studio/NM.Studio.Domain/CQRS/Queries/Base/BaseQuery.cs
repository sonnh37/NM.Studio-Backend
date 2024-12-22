using System.ComponentModel.DataAnnotations;
using MediatR;
using NM.Studio.Domain.Enums;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;

namespace NM.Studio.Domain.CQRS.Queries.Base;

public abstract class BaseQuery
{
}

public class GetQueryableQuery : BaseQuery
{
    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }


    [Required]
    public bool IsPagination { get; set; } = ConstantHelper.IsPagination;

    public Guid? Id { get; set; }

    public string? CreatedBy { get; set; }

    public string? LastUpdatedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public int PageNumber { get; set; } = ConstantHelper.PageNumberDefault;

    public int PageSize { get; set; } = ConstantHelper.PageSizeDefault;

    public string? SortField { get; set; } = ConstantHelper.SortFieldDefault;

    public SortOrder? SortOrder { get; set; } = ConstantHelper.SortOrderDefault;
}


public class GetByIdQuery : BaseQuery, IRequest<BusinessResult>
{
    public Guid Id { get; set; }
}

public class GetAllQuery : GetQueryableQuery, IRequest<BusinessResult>
{
}