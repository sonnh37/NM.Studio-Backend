using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Contracts.UnitOfWorks;
using NM.Studio.Domain.Entities;
using NM.Studio.Domain.Models.CQRS.Commands.MediaBases;
using NM.Studio.Domain.Models.Requests;
using NM.Studio.Domain.Models.Results;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;

namespace NM.Studio.Services;

public class MediaUploadService : IMediaUploadService
{
    private readonly Cloudinary _cloudinary;
    private readonly IUserContextService _userContext;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;

    public MediaUploadService(IUnitOfWork unitOfWork, IUserContextService userContext,
        IHttpContextAccessor httpContextAccessor)
    {
        var cloudinaryUrl = Environment.GetEnvironmentVariable("CLOUDINARY_URL");
        var cloudinary = new Cloudinary(cloudinaryUrl);
        cloudinary.Api.Secure = true;
        _cloudinary = cloudinary;
        _httpContextAccessor = httpContextAccessor;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<BusinessResult> UploadFile(FileUploadRequest request)
    {
        var userIdClaim = _userContext.GetUserId();
        if (userIdClaim == null)
            throw new UnauthorizedException("You need to authenticate with TeamMatching.");
        
        if (request.File == null) throw new DomainException("File is null");

        if (request.File.Length == 0)
            throw new DomainException("File is empty");

        if (request.File.Length > 10485760)
            throw new DomainException("File size exceeds 10MB limit.");

        var mediaBase = await UploadFileAsync(request, userIdClaim.Value);
        if (mediaBase == null)
            throw new DomainException("Failed to update file");

        return new BusinessResult(mediaBase);
    }

    public async Task<BusinessResult> UpdateFile(FileUpdateRequest request)
    {
        var userIdClaim = _userContext.GetUserId();
        if (userIdClaim == null)
            throw new UnauthorizedException("You need to authenticate with TeamMatching.");

        if (request.File == null) throw new DomainException("File is null");

        if (request.File.Length == 0)
            throw new DomainException("File is empty");

        if (request.File.Length > 10485760)
            throw new DomainException("File size exceeds 10MB limit.");

        var mediaBase = await ModifyFileAsync(request, userIdClaim.Value);
        if (mediaBase == null)
            throw new DomainException("Failed to update file");

        return new BusinessResult(mediaBase);
    }

    private async Task<MediaBase?> ModifyFileAsync(FileUpdateRequest request, Guid userId)
    {
        try
        {
            var uploadParams = BuildUploadFile(request, userId);
            if (uploadParams == null)
                return null;

            RawUploadResult uploadResult;

            // Upload dựa trên loại params thực tế
            if (uploadParams is ImageUploadParams imageParams)
            {
                uploadResult = await _cloudinary.UploadAsync(imageParams);
            }
            else if (uploadParams is VideoUploadParams videoParams)
            {
                uploadResult = await _cloudinary.UploadAsync(videoParams);
            }
            else
            {
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            if (uploadResult.Error != null)
            {
                throw new DomainException($"Upload failed: {uploadResult.Error.Message}");
            }

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK) throw new DomainException("Unknown error");

            // Update media base
            var mediaBase = await _unitOfWork.MediaBaseRepository.GetQueryable(m => m.Id == request.MediaId)
                .SingleOrDefaultAsync();
            if (mediaBase == null) throw new NotFoundException("Not found media base");

            var resource = await GetResourceAsync(uploadResult.SecureUrl.ToString());
            if (resource == null) throw new NotFoundException("Not found resource on cloudinary by src");

            mediaBase.DisplayName = resource.DisplayName ?? resource.PublicId;
            mediaBase.Title = resource.PublicId;
            mediaBase.MimeType = resource.Format;
            mediaBase.Size = resource.Bytes;
            mediaBase.Width = resource.Width;
            mediaBase.Height = resource.Height;
            mediaBase.MediaUrl = resource.SecureUrl;
            mediaBase.TakenMediaDate = !string.IsNullOrWhiteSpace(resource.CreatedAt)
                ? DateTimeOffset.Parse(resource.CreatedAt, null, System.Globalization.DateTimeStyles.AssumeUniversal)
                : null;
            mediaBase.MediaBaseType = resource.ResourceType == ResourceType.Image ? MediaBaseType.Image : MediaBaseType.Video;
            _unitOfWork.MediaBaseRepository.Update(mediaBase);
            if (!await _unitOfWork.SaveChanges()) return null;
            return mediaBase;
        }
        catch (Exception ex)
        {
            throw new Exception($"File upload failed: {ex.Message}", ex);
        }
    }

    private async Task<MediaBase?> UploadFileAsync(FileUploadRequest request, Guid userId)
    {
        try
        {
            var uploadParams = BuildUploadFile(request, userId);
            if (uploadParams == null)
                return null;

            RawUploadResult uploadResult;

            // Upload dựa trên loại params thực tế
            if (uploadParams is ImageUploadParams imageParams)
            {
                uploadResult = await _cloudinary.UploadAsync(imageParams);
            }
            else if (uploadParams is VideoUploadParams videoParams)
            {
                uploadResult = await _cloudinary.UploadAsync(videoParams);
            }
            else
            {
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            if (uploadResult.Error != null)
            {
                throw new DomainException($"Upload failed: {uploadResult.Error.Message}");
            }

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK) throw new DomainException("Unknown error");
            var mediaBaseResult = await CreateMediaBaseFromSrc(uploadResult.SecureUrl.ToString());
            if (mediaBaseResult == null) throw new DomainException("Failed to create media base");

            return mediaBaseResult;
        }
        catch (Exception ex)
        {
            throw new Exception($"File upload failed: {ex.Message}", ex);
        }
    }

    private RawUploadParams? BuildUploadFile(FileUploadRequest request, Guid userId)
    {
        var extension = request.GetExtensionFile().ToLowerInvariant();

        // Xác định loại file
        bool isImage = extension switch
        {
            ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".webp" or ".svg" => true,
            _ => false
        };

        bool isVideo = extension switch
        {
            ".mp4" or ".mov" or ".avi" or ".mkv" or ".webm" or ".wmv" => true,
            _ => false
        };

        if (!isImage && !isVideo)
        {
            // Không phải ảnh hay video, trả về null
            return null;
        }

        var stream = request.File.OpenReadStream();
        var publicId = $"{userId}/{Path.GetFileNameWithoutExtension(request.File.FileName)}";
        var folder = !string.IsNullOrEmpty(request.FolderName) ? $"Signed/{request.FolderName}" : "Signed";

        RawUploadParams rawUploadParams;

        // Tạo instance phù hợp dựa trên loại file
        if (isImage)
        {
            var imageParams = new ImageUploadParams
            {
                File = new FileDescription(request.File.FileName, stream),
                PublicId = publicId,
                Folder = folder,
                Overwrite = true,
                DisplayName = Path.GetFileNameWithoutExtension(request.File.FileName),
                Context = new StringDictionary
                {
                    ["alt"] = Path.GetFileNameWithoutExtension(request.File.FileName),
                    ["caption"] = Path.GetFileNameWithoutExtension(request.File.FileName)
                },
                Tags = !string.IsNullOrEmpty(request.FolderName) ? $"{request.FolderName},{userId}" : userId.ToString(),
                Transformation = new Transformation()
                    .Quality("auto")
                    .FetchFormat("auto")
            };
            rawUploadParams = imageParams;
        }
        else // isVideo
        {
            var videoParams = new VideoUploadParams
            {
                File = new FileDescription(request.File.FileName, stream),
                PublicId = publicId,
                Folder = folder,
                Overwrite = true,
                DisplayName = Path.GetFileNameWithoutExtension(request.File.FileName),
                Context = new StringDictionary
                {
                    ["alt"] = Path.GetFileNameWithoutExtension(request.File.FileName),
                    ["caption"] = Path.GetFileNameWithoutExtension(request.File.FileName)
                },
                Tags = !string.IsNullOrEmpty(request.FolderName) ? $"{request.FolderName},{userId}" : userId.ToString(),
                Transformation = new Transformation()
                    .VideoCodec("auto")
                    .FetchFormat("mp4")
            };
            rawUploadParams = videoParams;
        }

        return rawUploadParams;
    }

    public async Task<BusinessResult> DeleteFileAsync(string src)
    {
        if (string.IsNullOrWhiteSpace(src))
            throw new DomainException("Source cannot be empty.");

        // Extract PublicId từ src (Cloudinary link)
        var uri = new Uri(src);
        var path = uri.AbsolutePath;
        // path dạng "/Signed/folderName/userId/filename.jpg"
        // loại bỏ dấu "/" đầu tiên
        var publicId = path.TrimStart('/');
        // cắt bỏ phần mở rộng
        publicId = Path.Combine(
            Path.GetDirectoryName(publicId) ?? string.Empty,
            Path.GetFileNameWithoutExtension(publicId)
        ).Replace("\\", "/");

        var deletionParams = new DeletionParams(publicId)
        {
            ResourceType = ResourceType.Auto
        };

        var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

        if (deletionResult.Result == "ok")
            return new BusinessResult(deletionResult);

        throw new DomainException($"Failed to delete file: {deletionResult.Result}");
    }

    public async Task<GetResourceResult?> GetResourceAsync(string src)
    {
        if (string.IsNullOrWhiteSpace(src))
            throw new DomainException("Source cannot be empty.");

        const string cloudinaryPrefix = "https://res.cloudinary.com/";
        const string signedPrefix = "Signed/";

        if (!src.StartsWith(cloudinaryPrefix, StringComparison.OrdinalIgnoreCase))
            throw new DomainException("Invalid Cloudinary URL format.");

        // Cắt từ phần "Signed/" trở đi
        var startIndex = src.IndexOf(signedPrefix, StringComparison.OrdinalIgnoreCase);
        if (startIndex == -1)
            throw new DomainException("Missing Signed/ path in Cloudinary URL.");

        var publicIdWithExt = src[startIndex..];
        var publicId = Path.ChangeExtension(publicIdWithExt, null)?.Replace("\\", "/");

        return await _cloudinary.GetResourceAsync(new GetResourceParams(publicId));
    }

    private async Task<BusinessResult> HandleResult(UploadResult result)
    {
        if (result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            var mediaBaseResult = await CreateMediaBaseFromSrc(result.SecureUrl.ToString());
            if (mediaBaseResult == null) throw new DomainException("Failed to create media base");
            return new BusinessResult(mediaBaseResult);
        }

        throw new DomainException("Unknown error");
    }

    private async Task<MediaBase?> CreateMediaBaseFromSrc(string? src)
    {
        if (string.IsNullOrEmpty(src)) return null;
        MediaBase? entity = null;
        var getResourceResult = await GetResourceAsync(src);
        if (getResourceResult == null) throw new NotFoundException("Not found resource on cloudinary by src");
        entity = FromCloudinaryResource(getResourceResult);
        _unitOfWork.MediaBaseRepository.Add(entity);

        var saveChanges = await _unitOfWork.SaveChanges();
        if (!saveChanges)
            return null;

        return entity;
    }

    private MediaBase FromCloudinaryResource(GetResourceResult resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));

        var mediaBase = new MediaBase
        {
            DisplayName = resource.DisplayName ?? resource.PublicId,
            Title = resource.PublicId,
            MimeType = resource.Format,
            Size = resource.Bytes,
            Width = resource.Width,
            Height = resource.Height,
            MediaUrl = resource.SecureUrl,
            TakenMediaDate = !string.IsNullOrWhiteSpace(resource.CreatedAt)
                ? DateTimeOffset.Parse(resource.CreatedAt, null, System.Globalization.DateTimeStyles.AssumeUniversal)
                : null,
            MediaBaseType = resource.ResourceType == ResourceType.Image ? MediaBaseType.Image : MediaBaseType.Video,
            CreatedDate = DateTimeOffset.UtcNow
        };

        return mediaBase;
    }
}