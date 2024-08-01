using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CustomerApi;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)    
    {
        try
        {
           await _next(context);

        }
        catch(KeyNotFoundException ex){
             context.Response.ContentType = "text/plain; charset=utf-8 ";
             context.Response.StatusCode = (int)HttpStatusCode.NotFound;
             await context.Response.WriteAsync(ex.Message);
        }
        catch(DuplicateNameException ex){
             context.Response.ContentType = "text/plain; charset=utf-8 ";
             context.Response.StatusCode = (int)HttpStatusCode.Conflict;
             await context.Response.WriteAsync(ex.Message);
        }
        catch(AutoMapperMappingException ex){
            _logger.LogError(ex, $"Error occured in mapping : {ex.Message} ");
            context.Response.ContentType = "text/plain; charset=utf-8 ";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(ex.Message);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, $"Error occured: {ex.Message}");
            context.Response.ContentType = "text/plain; charset=utf-8 ";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(ex.Message);
        }
    }

}
