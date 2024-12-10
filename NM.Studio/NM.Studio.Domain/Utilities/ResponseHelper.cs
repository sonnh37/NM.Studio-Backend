using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Utilities;

public static class ResponseHelper
{
    #region Queries
    
    public static BusinessResult GetOne<TResult>(TResult? result)
        where TResult : BaseResult
    {
        if (result == null)
        {
            return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, null);
        }

        return new BusinessResult(Const.SUCCESS_CODE, Const.SUCCESS_READ_MSG, result);
    }

    public static BusinessResult GetAll<TResult>(List<TResult>? results)
        where TResult : class
    {
        if (results == null || results.Count == 0)
        {
            var response = new ResultsResponse<TResult>(results);
            return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, response);
        }

        var res = new ResultsResponse<TResult>(results);
        return new BusinessResult(Const.SUCCESS_CODE, Const.SUCCESS_READ_MSG, res);
    }
    
    public static BusinessResult GetAllPaginated<TResult>((List<TResult>? List, int? TotalCount) item,
        GetQueryableQuery pagedQuery)
        where TResult : BaseResult  
    {
        if (item.List == null || item.List.Count == 0)
        {
            var response = new PagedResponse<TResult>(pagedQuery);
            return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, response);
        }

        var res = new PagedResponse<TResult>(pagedQuery, item.List, item.TotalCount);
        return new BusinessResult(Const.SUCCESS_CODE, Const.SUCCESS_READ_MSG, res);
    }

    public static BusinessResult Error(string e)
    {
        return new BusinessResult(Const.ERROR_EXCEPTION_CODE, e);
    }

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
    public static BusinessResult Save<TResult>(TResult? result)
        where TResult : BaseResult
    {
        if (result == null)
        {
            return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, null);
        }

        return new BusinessResult(Const.SUCCESS_CODE, Const.SUCCESS_SAVE_MSG, result);
    }
    
    public static BusinessResult Delete(bool isDeleted)
    {
        if (!isDeleted)
        {
            return new BusinessResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
        }

        return new BusinessResult(Const.SUCCESS_CODE, Const.SUCCESS_DELETE_MSG);
    }
    #endregion
}