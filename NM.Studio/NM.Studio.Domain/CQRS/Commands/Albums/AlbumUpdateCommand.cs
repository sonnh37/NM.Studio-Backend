﻿using NM.Studio.Domain.CQRS.Commands.Base;

namespace NM.Studio.Domain.CQRS.Commands.Albums;

public class AlbumUpdateCommand : UpdateCommand
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Background { get; set; }
}