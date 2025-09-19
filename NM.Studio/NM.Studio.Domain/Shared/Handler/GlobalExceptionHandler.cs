using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NM.Studio.Domain.Models.Results.Bases;
using NM.Studio.Domain.Shared.Exceptions;

namespace NM.Studio.Domain.Shared.Handler;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            $"An error occurred while processing your request: {exception.Message}");

        var activity = Activity.Current;
        ProblemDetails problem = exception switch
        {
            NotFoundException => BuildProblemDetails(exception, httpContext, activity, StatusCodes.Status404NotFound,
                "Not Found"),
            DomainException => BuildProblemDetails(exception, httpContext, activity, StatusCodes.Status400BadRequest,
                "Domain Error"),
            ApiException => BuildProblemDetails(exception, httpContext, activity,
                StatusCodes.Status500InternalServerError, "API Error"),
            UnauthorizedException => BuildProblemDetails(exception, httpContext, activity,
                StatusCodes.Status401Unauthorized, "Unauthorized"),
            _ => BuildProblemDetails(exception, httpContext, activity, StatusCodes.Status500InternalServerError,
                "Internal Server Error")
        };

        activity?.SetStatus(ActivityStatusCode.Error, exception.Message);
        httpContext.Response.StatusCode = (int)problem.Status!;

        var businessResult = new BusinessResult
        {
            Status = problem.Status == StatusCodes.Status200OK ? Status.OK : Status.ERROR,
            Error = problem,
            Data = null,
            TraceId = activity?.TraceId.ToString()
        };

        await httpContext
            .Response
            .WriteAsJsonAsync(businessResult, cancellationToken);

        return true;
    }

    private static ProblemDetails BuildProblemDetails(
        Exception exception,
        HttpContext httpContext,
        Activity? activity,
        int statusCode,
        string title)
    {
        var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        var problem = new ProblemDetails
        {
            Type = exception.GetType().Name,
            Title = title,
            Status = statusCode,
            Detail = isDev
                ? exception.Message
                : (statusCode >= 500
                    ? "Hệ thống có lỗi, vui lòng thử lại sau!"
                    : exception.Message),
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        };

        problem.Extensions.Add("traceId", activity?.TraceId.ToString());

        if (isDev)
        {
            problem.Extensions.Add("exception", exception.GetType().FullName);
            problem.Extensions.Add("stackTrace", exception.StackTrace);
            problem.Extensions.Add("innerMessage", exception.InnerException?.Message);
            problem.Extensions.Add("innerStack", exception.InnerException?.StackTrace);
        }

        return problem;
    }
}