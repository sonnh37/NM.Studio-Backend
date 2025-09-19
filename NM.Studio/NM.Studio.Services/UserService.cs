using AutoMapper;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NM.Studio.Domain.Contracts.Repositories;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.CQRS.Commands.Base;
using NM.Studio.Domain.CQRS.Commands.Users;
using NM.Studio.Domain.CQRS.Queries.Users;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;
using NM.Studio.Domain.Utilities.Filters;
using NM.Studio.Services.Bases;
using OtpNet;

namespace NM.Studio.Services;

public class UserService : BaseService, IUserService
{
    private readonly string _clientId;
    private readonly IUserContextService _userContextService;
    private readonly IUserRepository _userRepository;

    public UserService(IMapper mapper,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        IUserContextService userContextService)
        : base(mapper, unitOfWork)
    {
        _userContextService = userContextService;
        _userRepository = _unitOfWork.UserRepository;
    }

    public async Task<BusinessResult> GetById(UserGetByIdQuery request)
    {
        var entity = await _userRepository.GetQueryable(m => m.Id == request.Id).SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException("Not found");
        var result = _mapper.Map<ServiceResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> UpdatePassword(UserPasswordCommand userPasswordCommand)
    {
        var userId = _userContextService.GetUserId();
        if (userId == null) throw new DomainException("Please, login again.");

        var user = await _userRepository.GetQueryable(m => m.Id == userId).SingleOrDefaultAsync();
        if (user == null) throw new DomainException("Account not in the system");
        user.Password = userPasswordCommand.Password;
        _userRepository.Update(user);
        if (!await _unitOfWork.SaveChanges()) throw new Exception();
        var userResult = _mapper.Map<UserResult>(user);

        return new BusinessResult(userResult);
    }

    public async Task<BusinessResult> UpdateUserCacheAsync(UserUpdateCacheCommand newCacheJson)
    {
        var userId = _userContextService.GetUserId();
        if (userId == null) throw new DomainException("Please, login again.");

        var user = await _userRepository.GetQueryable(m => m.Id == userId).SingleOrDefaultAsync();
        if (user == null) throw new DomainException("Account not in the system");

        // JObject existingCache;
        // try
        // {
        //     existingCache = string.IsNullOrWhiteSpace(user.Cache) ? new JObject() : JObject.Parse(user.Cache);
        // }
        // catch (JsonReaderException ex)
        // {
        //     Console.WriteLine($"Error parsing cache: {ex.Message}, Raw Data: {user.Cache}");
        //     existingCache = new JObject(); // Nếu lỗi thì dùng cache mới
        // }
        //
        // if (newCacheJson.Cache != null)
        // {
        //     var newCache = JObject.Parse(newCacheJson.Cache);
        //
        //     existingCache.Merge(newCache, new JsonMergeSettings
        //     {
        //         MergeArrayHandling = MergeArrayHandling.Union
        //     });
        // }
        //
        // user.Cache = existingCache.ToString();
        // _userRepository.Update(user);
        // var isSaveChanges = await _unitOfWork.SaveChanges();
        // if (!isSaveChanges)
        //     throw new Exception();

        return new BusinessResult();
    }

    public async Task<BusinessResult> GetAll(UserGetAllQuery query)
    {
        var queryable = _userRepository.GetQueryable();

        queryable = FilterHelper.BaseEntity(queryable, query);
        queryable = RepoHelper.Include(queryable, query.IncludeProperties);
        queryable = RepoHelper.Sort(queryable, query);

        var totalCount = await queryable.CountAsync();
        var entities = await RepoHelper.GetQueryablePagination(queryable, query).ToListAsync();
        var results = _mapper.Map<List<UserResult>>(entities);
        var getQueryableResult = new GetQueryableResult(results, totalCount, query);

        return new BusinessResult(getQueryableResult);
    }

    public async Task<BusinessResult> CreateOrUpdate(CreateOrUpdateCommand createOrUpdateCommand)
    {
        User? entity = null;
        if (createOrUpdateCommand is UserUpdateCommand updateCommand)
        {
            entity = await _userRepository.GetQueryable(m => m.Id == updateCommand.Id).SingleOrDefaultAsync();

            if (entity == null)
                throw new NotFoundException(Const.NOT_FOUND_MSG);

            _mapper.Map(updateCommand, entity);
            _userRepository.Update(entity);
        }
        else if (createOrUpdateCommand is UserCreateCommand createCommand)
        {
            createCommand.Password = BCrypt.Net.BCrypt.HashPassword(createCommand.Password);
            entity = _mapper.Map<User>(createCommand);
            entity.Role = Role.Customer;
            entity.Status = UserStatus.Active;
            if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
            entity.CreatedDate = DateTime.UtcNow;
            _userRepository.Add(entity);
        }

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        var result = _mapper.Map<UserResult>(entity);

        return new BusinessResult(result);
    }

    public async Task<BusinessResult> Delete(UserDeleteCommand command)
    {
        var entity = await _userRepository.GetQueryable(x => x.Id == command.Id).SingleOrDefaultAsync();
        if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);

        _userRepository.Delete(entity, command.IsPermanent);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            throw new Exception();

        return new BusinessResult();
    }

    private string GenerateSecretKey(int length)
    {
        var secretKey = KeyGeneration.GenerateRandomKey(length);
        return Base32Encoding.ToString(secretKey);
    }

    private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string token)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new List<string> { _clientId }
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
        return payload;
    }
}