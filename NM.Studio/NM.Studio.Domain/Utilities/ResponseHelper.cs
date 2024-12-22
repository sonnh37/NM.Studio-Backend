using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Utilities;

public static class ResponseHelper
{
    #region Success

    public static BusinessResult Success(string message = Const.SUCCESS_READ_MSG)
    {
        return new BusinessResult(Const.SUCCESS_CODE, message);
    }
    
    public static BusinessResult Success<TResult>(List<TResult>? results)
        where TResult : class
    {
        var res = new ResultsResponse<TResult>(results);
        return new BusinessResult(Const.SUCCESS_CODE, Const.SUCCESS_READ_MSG, res);
    }

    public static BusinessResult Success<TResult>(TResult? result)
        where TResult : BaseResult
    {
        return new BusinessResult(Const.SUCCESS_CODE, Const.SUCCESS_READ_MSG, result);
    }

    public static BusinessResult Success<TResult>((List<TResult>? List, int? TotalCount) item,
        GetQueryableQuery pagedQuery)
        where TResult : BaseResult
    {
        var res = new PagedResponse<TResult>(pagedQuery, item.List, item.TotalCount);
        return new BusinessResult(Const.SUCCESS_CODE, Const.SUCCESS_READ_MSG, res);
    }

    #endregion

    #region Warning

    public static BusinessResult Warning(string message = Const.WARNING_NO_DATA_MSG)
    {
        return new BusinessResult(Const.WARNING_NO_DATA_CODE, message);
    }
    
    public static BusinessResult Warning<TResult>(List<TResult>? results)
        where TResult : class
    {
        var response = new ResultsResponse<TResult>(results);
        return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, response);
    }

    public static BusinessResult Warning<TResult>(TResult? result)
        where TResult : BaseResult
    {
        return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, result);
    }

    public static BusinessResult Warning<TResult>(
        GetQueryableQuery pagedQuery)
        where TResult : BaseResult
    {
        var response = new PagedResponse<TResult>(pagedQuery);
        return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, response);
    }

    #endregion

    #region NotFound

    public static BusinessResult NotFound(string message = Const.NOT_FOUND_MSG)
    {
        return new BusinessResult(Const.NOT_FOUND_CODE, message);
    }
    
    #endregion

    #region Error
    public static BusinessResult Error(string? e = "Error system")
    {
        return new BusinessResult(Const.ERROR_EXCEPTION_CODE, e);
    }
    #endregion

    #region Queries

    public static BusinessResult GetToken(string? token, string? expiration, string? msg = null)
    {
        if (token == null && expiration == null && msg != null)
        {
            var response = new LoginResponse(null, null);
            return new BusinessResult(Const.NOT_FOUND_CODE, msg, response);
        }

        var res = new LoginResponse(token, expiration);
        return new BusinessResult(Const.SUCCESS_CODE, Const.SUCCESS_LOGIN_MSG, res);
    }

    #endregion

    #region Commands

    public static BusinessResult Delete(bool isDeleted)
    {
        return !isDeleted ? new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG) : new BusinessResult(Const.SUCCESS_CODE, Const.SUCCESS_DELETE_MSG);
    }

    #endregion
}