using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.Requests;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;
using NM.Studio.Domain.Utilities;

namespace NM.Studio.Services;

public class MediaUploadService : IMediaUploadService
{
    private readonly Cloudinary _cloudinary;
    private readonly IUserContextService _userContext;
    protected readonly IHttpContextAccessor _httpContextAccessor;

    public MediaUploadService(IUserContextService userContext, IHttpContextAccessor httpContextAccessor)
    {
        var cloudinaryUrl = Environment.GetEnvironmentVariable("CLOUDINARY_URL");
        var cloudinary = new Cloudinary(cloudinaryUrl);
        cloudinary.Api.Secure = true;
        _cloudinary = cloudinary;
        _httpContextAccessor = httpContextAccessor;
        _userContext = userContext;
    }

    public async Task<BusinessResult> UploadFile(FileUploadRequest request)
    {
        var userIdClaim = _userContext.GetUserId();
        if (userIdClaim == null)
            throw new UnauthorizedException("You need to authenticate with TeamMatching.");

        if (request.File.Length == 0)
            throw new DomainException("File is empty");

        if (request.File.Length > 10485760)
            throw new DomainException("File size exceeds 10MB limit.");

        var isImage = request.File.ContentType.StartsWith("image/");

        return isImage
            ? await UploadImageAsync(request, userIdClaim.Value)
            : await UploadRawAsync(request, userIdClaim.Value);
    }

    private async Task<BusinessResult> UploadImageAsync(FileUploadRequest request, Guid userId)
    {
        using var stream = request.File.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(userId.ToString(), stream),
            PublicId = $"{userId}/{Path.GetFileNameWithoutExtension(request.File.FileName)}",
            Folder = $"Signed/{request.FolderName}",
            Overwrite = true
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return HandleResult(uploadResult);
    }

    private async Task<BusinessResult> UploadRawAsync(FileUploadRequest request, Guid userId)
    {
        using var stream = request.File.OpenReadStream();
        var uploadParams = new RawUploadParams
        {
            File = new FileDescription(userId.ToString(), stream),
            PublicId = $"{userId}/{Path.GetFileNameWithoutExtension(request.File.FileName)}",
            Folder = $"Signed/{request.FolderName}",
            Overwrite = true
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return HandleResult(uploadResult);
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

    // public async Task<BusinessResult> Delete(Guid command)
    // {
    //     var queryable = _blogRepository.GetQueryable(x => x.Id == command.Id);
    //     queryable = RepoHelper.Include(queryable, ["thumbnail.image.mediaUrl"]);
    //     var entity = await queryable.SingleOrDefaultAsync();
    //
    //     if (entity == null) throw new NotFoundException(Const.NOT_FOUND_MSG);
    //
    //     _blogRepository.Delete(entity, command.IsPermanent);
    //
    //     var saveChanges = await _unitOfWork.SaveChanges();
    //     if (!saveChanges)
    //         throw new Exception();
    //     
    //     var src = entity.Thumbnail?.MediaUrl;
    //     if (!string.IsNullOrEmpty(src))
    //     {
    //         var res = await _mediaUploadService.DeleteFileAsync(src);
    //     }
    //
    //     return new BusinessResult();
    // }

    private BusinessResult HandleResult(UploadResult result)
    {
        if (result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return new BusinessResult(result);
        }

        throw new DomainException("Unknown error");
    }
}