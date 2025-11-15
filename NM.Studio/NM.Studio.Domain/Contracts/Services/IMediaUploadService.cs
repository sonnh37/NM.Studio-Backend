using CloudinaryDotNet.Actions;
using NM.Studio.Domain.Models.Requests;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Contracts.Services;

public interface IMediaUploadService
{
    Task<BusinessResult> UploadFile(FileUploadRequest request);
    Task<BusinessResult> UpdateFile(FileUpdateRequest request);
    Task<BusinessResult> DeleteFileAsync(string src);
    Task<GetResourceResult?> GetResourceAsync(string src);
}