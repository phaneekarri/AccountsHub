using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CustomerApi.Controllers;

[ApiController]
[Route("/api/v1/Account/{Id}/owners")]
public class AccountOwnerController(
    ILogger<AccountOwnerController> logger,
    IAccountService service)
 : ControllerBase
{

    [HttpPost("batch")]
    public async Task<ActionResult> CreateAccountOwners(int Id, [FromBody] 
    IEnumerable<CreateAccountOwner> owners)
    {
        logger.LogTrace("Create Account Owners is running...");
        return await service.AddOwners(Id, owners)? Ok() : NoContent();    
    }

    [HttpPost]
    public async Task<ActionResult> CreateAccountOwner(int Id, 
    [FromBody] CreateAccountOwner owner)
    {
        logger.LogTrace("CreateAccountOwner is running..");
        return await service.AddOwners(Id, new List<CreateAccountOwner>{owner})? Ok() : NoContent();    
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteAccountOwners(int Id)
    {
        logger.LogTrace("Delete all account Owners");    
        return await service.DeleteOwners(Id)? NoContent()  : StatusCode(500, "An unexpected error occurred."); ; 
    }

    [HttpDelete("{ClientId}")]
    public async Task<ActionResult> DeleteAccountOwnersByClient(int Id, int ClientId)
    {
        logger.LogTrace("Delete account owner by client Id is running..");
        return await service.DeleteOwner(Id, ClientId)? NoContent()  : StatusCode(500, "An unexpected error occurred."); ; 
    }
}