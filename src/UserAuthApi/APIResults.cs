using Infra;

namespace UserAuthApi;

public static class APIResults
{    

    public static IResult Ok<T>(this T data, string? message = null)
    => Results.Ok(new APIResponse<T>(StatusCodes.Status200OK, data, message??"Success"));
    
    public static IResult NotFound(string? message = null)
    => Results.NotFound(new APIResponse(StatusCodes.Status404NotFound, message??"Not Found"));

    public static IResult NotFound<T>(this T data, string? message = null)
    => Results.NotFound(new APIResponse<T>(StatusCodes.Status404NotFound, data, message??"Not Found"));

    public static IResult Created(string? uri) 
    => Results.Created(uri, new APIResponse(StatusCodes.Status201Created));
    public static IResult Created<T>(this T data, string? uri,  string? message = null)
    => Results.Created(uri, new APIResponse<T>(StatusCodes.Status201Created, data, message??"Created"));

    public static IResult Conflict(string? message = null)
    => Results.Conflict(new APIResponse(StatusCodes.Status201Created, message??"Conflict exists"));
    

    public static RouteHandlerBuilder Produces<T>(this RouteHandlerBuilder route, int statusCode)
     => OpenApiRouteHandlerBuilderExtensions.Produces<APIResponse<T>>(route, statusCode);
    public static RouteHandlerBuilder Produces(this RouteHandlerBuilder route, int statusCode)
     => OpenApiRouteHandlerBuilderExtensions.Produces<APIResponse>(route, statusCode);
     
}

