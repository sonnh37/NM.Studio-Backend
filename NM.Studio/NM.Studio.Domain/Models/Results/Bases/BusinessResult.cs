using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace NM.Studio.Domain.Models.Results.Bases;

public class BusinessResult
{
    public BusinessResult()
    {
    }

    public BusinessResult(object? data)
    {
        Data = data;
        Status = data == null ? "ERROR" : "OK";
    }
    
    public string Status { get; set; } = "OK";
    public ProblemDetails? Error { get; set; }
    public object? Data { get; set; }
    public string? TraceId { get; set; } = Activity.Current?.TraceId.ToString();
    
}

public enum Status
{
    OK,
    ERROR
}