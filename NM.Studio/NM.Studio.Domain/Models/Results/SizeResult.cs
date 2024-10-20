﻿using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Models.Results;

public class SizeResult : BaseResult
{
    public string? Name { get; set; }

    public List<OutfitResult> Outfits { get; set; } = new();
}