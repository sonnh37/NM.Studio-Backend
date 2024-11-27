using AutoMapper;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Albums;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Utilities;
using NM.Studio.Services.Bases;

namespace NM.Studio.Services;

public class AlbumService : BaseService<Album>, IAlbumService
{
    private readonly IAlbumRepository _albumRepository;

    public AlbumService(IMapper mapper,
        IUnitOfWork unitOfWork)
        : base(mapper, unitOfWork)
    {
        _albumRepository = _unitOfWork.AlbumRepository;
    }
    
    public async Task<BusinessResult> Create<TResult>(AlbumCreateCommand createCommand) where TResult : BaseResult
    {
        try
        {
            createCommand.Slug = SlugHelper.ToSlug(createCommand.Title);
            var album = _albumRepository.GetQueryable(m => m.Slug == createCommand.Slug).SingleOrDefault();
            if (album != null) return ResponseHelper.Error("Title is already in use"); 
            
            var entity = await CreateOrUpdateEntity(createCommand);
            var result = _mapper.Map<TResult>(entity);
            var msg = ResponseHelper.SaveData(result);
            
            return msg;
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred while updating {typeof(AlbumCreateCommand).Name}: {ex.Message}";
            return ResponseHelper.Error(errorMessage);
        }
    }
    
    public async Task<BusinessResult> Update<TResult>(AlbumUpdateCommand updateCommand) where TResult : BaseResult
    {
        try
        {
            updateCommand.Slug = SlugHelper.ToSlug(updateCommand.Title);
            var album = _albumRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefault();            
            
            if (album == null) throw new Exception();
            
            // check if update input slug != current slug
            if (updateCommand.Slug != album?.Slug)
            {
                // continue check if input slug == any slug
                var album_ = _albumRepository.GetQueryable(m => m.Slug == updateCommand.Slug).SingleOrDefault();

                if (album_ != null) return ResponseHelper.Error("Title is already in use"); 
            }
            
            var entity = await CreateOrUpdateEntity(updateCommand);
            var result = _mapper.Map<TResult>(entity);
            var msg = ResponseHelper.SaveData(result);
            
            return msg;
        }
        catch (Exception ex)
        {
            var errorMessage = $"An error occurred while updating {typeof(AlbumUpdateCommand).Name}: {ex.Message}";
            return ResponseHelper.Error(errorMessage);
        }
    }

}