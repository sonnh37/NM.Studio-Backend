using Microsoft.AspNetCore.Http;

namespace NM.Studio.Domain.Models.Requests;

public class FileUploadListRequest
{
    public List<IFormFile>? Files { get; set; }
    public string FolderName { get; set; }
    
    public List<string> GetExtensionFiles()
    {
        return Files != null ? Files.Select(file => Path.GetExtension(file.FileName)).ToList() : [];
    }
}