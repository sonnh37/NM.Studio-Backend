﻿using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Models.Results;

namespace NM.Studio.Domain.CQRS.Queries.Photos;

public class PhotoGetAllQuery : GetAllQuery<PhotoResult>
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Src { get; set; }

    public string? Href { get; set; }

    public string? Tag { get; set; }
}