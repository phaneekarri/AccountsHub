using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CustomerApi;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
: IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception,
        CancellationToken cancellationToken)
     {
        httpContext.Response.ContentType = "text/plain; charset=utf-8 ";
        var problemDetails = new ProblemDetails
        {
            Title = "An unexpected error occurred.",
            Detail = exception.Message,
            Status = (int)HttpStatusCode.InternalServerError,
            Instance = httpContext.Request.Path
        };
        if(exception is KeyNotFoundException)
        {
            problemDetails.Status = (int)HttpStatusCode.NotFound;
            problemDetails.Title = "Not Found";
            httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
        if(exception is DuplicateNameException)
        {
            problemDetails.Status = (int)HttpStatusCode.Conflict;
            problemDetails.Title = "Conflict";
            httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
        }
        logger.LogError(exception, "An unexpected error occurred.");
        httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return new ValueTask<bool>(true);
     }

}
