﻿using CMS.Studio.Domain.Contracts.Services;
using CMS.Studio.Domain.CQRS.Queries.Outfits;
using CMS.Studio.Domain.Models.Responses;
using CMS.Studio.Domain.Models.Results;

using MediatR;

namespace CMS.Studio.Handler.Queries;

public class OutfitQueryHandler:
    IRequestHandler<OutfitGetAllQuery, TableResponse<OutfitResult>>,
    IRequestHandler<OutfitGetByIdQuery, ItemResponse<OutfitResult>>
{
    protected readonly IOutfitService _outfitService;

    public OutfitQueryHandler(IOutfitService outfitService) 
    {
        _outfitService = outfitService;
    }

    public async Task<TableResponse<OutfitResult>> Handle(OutfitGetAllQuery request,
        CancellationToken cancellationToken)
    {
        return await _outfitService.GetAll<OutfitResult>(request);
    }

    public async Task<ItemResponse<OutfitResult>> Handle(OutfitGetByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _outfitService.GetById<OutfitResult>(request.Id);
    }
}