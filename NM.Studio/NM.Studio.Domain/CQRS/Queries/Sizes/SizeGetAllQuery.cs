﻿using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Domain.CQRS.Queries.Sizes;

public class SizeGetAllQuery : GetAllQuery
{
    public string? Name { get; set; }

    public Guid? ProductId {
        get;
        set;
    }
}