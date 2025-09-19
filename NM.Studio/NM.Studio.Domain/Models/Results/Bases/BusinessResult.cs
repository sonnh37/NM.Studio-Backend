using System.Diagnostics;

namespace NM.Studio.Domain.Models.Results.Bases;

public class BusinessResult
{
    public Status Status = Status.OK;

    public BusinessResult()
    {
    }

    public BusinessResult(object? data, string? message = "")
    {
        Data = data;
        Status = data == null ? Status.ERROR : Status.OK;
        Message = message;
    }

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