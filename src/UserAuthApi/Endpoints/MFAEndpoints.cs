using Microsoft.AspNetCore.Mvc;
using UserAuthApi.Process;

public static class MFAEndpoints
{
    public static void MapMFAEndpoints(this WebApplication app)
    {
        app.MapPost("/EnableMFA", 
        async([FromBody] string userId,
         IInternalUserLogin process)=>
        {
            if(!Guid.TryParse( userId, out Guid userGuid)) return Results.BadRequest("User is invalid");
            await process.EnableMFA(userGuid);
            return Results.Ok();
        })
        .WithName("EnableMFA")
        .WithOpenApi();
    }
}