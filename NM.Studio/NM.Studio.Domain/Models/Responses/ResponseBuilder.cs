using System.Collections;
using NM.Studio.Domain.CQRS.Queries.Base;

namespace NM.Studio.Domain.Models.Responses;

public class ResponseBuilder
{
    protected int _status;
    protected string _message;
    protected object? _data = null;
    // Các phương thức hỗ trợ method chaining
    public ResponseBuilder WithStatus(int status)
    {
        _status = status;
        return this;
    }

    public ResponseBuilder WithMessage(string message)
    {
        _message = message;
        return this;
    }

    public virtual BusinessResult Build()
    {
        return new BusinessResult(_status, _message);
    }
}

public class ResponseBuilder<TResult> : ResponseBuilder where TResult : class
{
    public ResponseBuilder<TResult> WithData(object? data)
    {
        _data = data;
        return this;
    }
    public override BusinessResult Build()
    {
        object? responseData = _data;

        if (_data is IEnumerable<TResult> enumerableData)
        {
            responseData = new ResultsResponse<TResult>(enumerableData);
        }
        else if (_data is ResultsTableResponse<TResult> tableResponse)
        {
            var item = tableResponse.Item!.Value;
            var getQueryableQuery = tableResponse.GetQueryableQuery;
            responseData = new PagedResponse<TResult>(getQueryableQuery!, item.Item1, item.Item2);
        }

        return new BusinessResult(_status, _message, responseData);
    }
}
