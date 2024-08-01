using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CustomerApi;

[ApiController]
[Route("/api/v1/Owner")]
public class AccountOwnerController(ILogger<AccountOwnerController> logger, IAccountOwnerService service) : ControllerBase
{
  private readonly ILogger<AccountOwnerController> _logger = logger;
  private readonly IAccountOwnerService _service = service;

  [HttpPost("List/{Id}")]
  public async Task<ActionResult> CreateAccountOwners(int Id, [FromBody] IEnumerable<CreateAccountOwner> owners)
  {
     _logger.LogInformation("Create Account Owners is running...");
     return await _service.CreateOwnersByAccount(Id, owners)? Ok() : NoContent();    
  }

  [HttpPost("{Id}")]
  public async Task<ActionResult> CreateAccountOwner(int Id, [FromBody] CreateAccountOwner owner)
  {
     _logger.LogInformation("CreateAccountOwner is running..");
     return await _service.CreateOwnersByAccount(Id, new List<CreateAccountOwner>{owner})? Ok() : NoContent();    
  }

  [HttpDelete("Account/{AccountId}/Client/{ClientId}")]
  public async Task<ActionResult> DeleteAccountOwnersByClient(int AccountId, int ClientId)
  {
   _logger.LogInformation("Delete account owner by client Id is running..");
     return await _service.DeleteAccountOwnersByClient(AccountId, ClientId)? NoContent()  : StatusCode(500, "An unexpected error occurred."); ; 
  }

  [HttpDelete("Account/{AccountId}")]
  public async Task<ActionResult> DeleteAccountOwners(int AccountId)
  {
     _logger.LogInformation("Delete all account Owners");    
     return await _service.DeleteAccountOwners(AccountId)? NoContent()  : StatusCode(500, "An unexpected error occurred."); ; 
  }
}
