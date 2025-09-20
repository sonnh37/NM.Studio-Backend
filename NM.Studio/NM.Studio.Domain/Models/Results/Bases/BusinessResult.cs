using System.Diagnostics;

namespace NM.Studio.Domain.Models.Results.Bases;

public class BusinessResult
{
    public BusinessResult()
    {
    }

    public BusinessResult(object? data, string? message = "")
    {
        Data = data;
        Status = data == null ? "ERROR" : "OK";
        Message = message;
    }
    
    public string Status { get; set; } = "OK";
    public string? Message { get; set; }
    public object? Error { get; set; }
    public object? Data { get; set; }
    public string? TraceId { get; set; } = Activity.Current?.TraceId.ToString();
    
}

public enum Status
{
    OK,
    ERROR
}