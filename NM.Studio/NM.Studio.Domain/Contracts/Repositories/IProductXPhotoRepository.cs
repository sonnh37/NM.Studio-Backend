﻿using NM.Studio.Domain.Contracts.Repositories.Bases;
using NM.Studio.Domain.CQRS.Commands.ProductXPhotos;
using NM.Studio.Domain.Entities;

namespace NM.Studio.Domain.Contracts.Repositories;

public interface IProductXPhotoRepository : IBaseRepository<ProductXPhoto>
{
    Task<ProductXPhoto?> GetById(ProductXPhotoDeleteCommand command);
    void Delete(ProductXPhoto entity);
}