﻿using AutoMapper;
using CMS.Studio.Data.Context;
using CMS.Studio.Data.Repositories.Base;
using CMS.Studio.Domain.Contracts.Repositories;
using CMS.Studio.Domain.CQRS.Queries.Photos;
using CMS.Studio.Domain.Entities;
using CMS.Studio.Domain.Utilities;

namespace CMS.Studio.Data.Repositories;

public class PhotoRepository : BaseRepository<Photo>, IPhotoRepository
{
    public PhotoRepository(StudioContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }

}