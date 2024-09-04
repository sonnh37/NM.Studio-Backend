﻿using CMS.Studio.Domain.Models.Results.Bases;

namespace CMS.Studio.Domain.Models.Results;

public class PhotoResult : BaseResult
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Type { get; set; }

    public string? Src { get; set; }

    public string? Href { get; set; }

    public Guid? AlbumId { get; set; }

    public Guid? OutfitId { get; set; }

    public AlbumResult? Album { get; set; }

    public OutfitResult? Outfit { get; set; }
}