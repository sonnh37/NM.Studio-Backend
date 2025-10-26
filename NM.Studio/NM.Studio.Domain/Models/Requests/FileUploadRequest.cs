using Microsoft.AspNetCore.Http;

namespace NM.Studio.Domain.Models.Requests;

public class FileUploadRequest
{
    public IFormFile? File { get; set; }
    public string FolderName { get; set; }
    
    public string GetExtensionFile()
    {
        var extension = Path.GetExtension(File.FileName);
        return extension;
    }
}