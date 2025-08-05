using NM.Studio.Domain.Utilities;

namespace NM.Studio.Domain.Models.Results.Bases;

public class BusinessResult
{
    public bool IsSuccess { get; set; }
    public int Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Error { get; set; }

    public object? Data { get; set; }


    public static BusinessResult Success(string message = "Success")
    {
        return new BusinessResult
        {
            IsSuccess = true,
            Status = Const.SUCCESS_CODE,
            Message = message
        };
    }

    public static BusinessResult Success(object data, string message = "Success")
    {
        return new BusinessResult
        {
            IsSuccess = true,
            Status = Const.SUCCESS_CODE,
            Message = message,
            Data = data
        };
    }

    public static BusinessResult Fail(string error = "Something went wrong...")
    {
        return new BusinessResult
        {
            IsSuccess = false,
            Status = Const.FAIL_CODE,
            Message = "Fail",
            Error = error
        };
    }

    public static BusinessResult ExceptionError(string error = "Something went wrong...")
    {
        return new BusinessResult
        {
            IsSuccess = false,
            Status = Const.ERROR_EXCEPTION_CODE,
            Message = "Exception error",
            Error = error
        };
    }
}