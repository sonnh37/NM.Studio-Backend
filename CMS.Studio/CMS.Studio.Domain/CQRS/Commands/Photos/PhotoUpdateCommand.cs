﻿using CMS.Studio.Domain.CQRS.Commands.Base;

namespace CMS.Studio.Domain.CQRS.Commands.Photos;

public class PhotoUpdateCommand : UpdateCommand
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Src { get; set; }

    public string? Href { get; set; }

    public string? Tag { get; set; }
}