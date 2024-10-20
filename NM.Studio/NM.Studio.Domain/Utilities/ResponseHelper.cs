﻿using NM.Studio.Domain.CQRS.Queries.Base;
using NM.Studio.Domain.Models.Responses;
using NM.Studio.Domain.Models.Results.Bases;

namespace NM.Studio.Domain.Utilities;

public static class ResponseHelper
{
    public static ItemListResponse<TResult> CreateItemList<TResult>(List<TResult>? results)
        where TResult : BaseResult
    {
        if (results == null) return new ItemListResponse<TResult>(ConstantHelper.Fail, results);

        if (!results.Any()) return new ItemListResponse<TResult>(ConstantHelper.NotFound, results);

        return new ItemListResponse<TResult>(ConstantHelper.Success, results);
    }

    public static TableResponse<TResult> CreateTable<TResult>((List<TResult>?, int?) item,
        GetQueryableQuery pagedQuery)
        where TResult : BaseResult
    {
        if (item.Item1 == null) return new TableResponse<TResult>(ConstantHelper.Fail, pagedQuery, item.Item1);

        if (!item.Item1.Any()) return new TableResponse<TResult>(ConstantHelper.NotFound, pagedQuery, item.Item1);

        if (item.Item2 == null)
        {
        }

        return new TableResponse<TResult>(ConstantHelper.Success, pagedQuery, item.Item1, item.Item2);
    }

    public static ItemResponse<TResult> CreateItem<TResult>(TResult? result)
        where TResult : BaseResult
    {
        var message = result != null ? ConstantHelper.Success : ConstantHelper.Fail;
        return new ItemResponse<TResult>(message, result);
    }

    public static LoginResponse<TResult> CreateLogin<TResult>(TResult? result, string? token, string? expiration)
        where TResult : BaseResult
    {
        var message = result != null ? ConstantHelper.Success : ConstantHelper.Fail;
        return new LoginResponse<TResult>(message, result, token, expiration);
    }

    public static MessageResponse CreateMessage(string message, bool isSuccess)
    {
        return new MessageResponse(isSuccess, message);
    }
}