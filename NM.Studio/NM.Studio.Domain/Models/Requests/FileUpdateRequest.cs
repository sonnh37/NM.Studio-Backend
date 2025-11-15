using Microsoft.AspNetCore.Http;

namespace NM.Studio.Domain.Models.Requests;

public class FileUpdateRequest : FileUploadRequest
{
    public Guid MediaId { get; set; }
}