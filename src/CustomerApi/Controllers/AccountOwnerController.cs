using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerApi.Dto;
using CustomerEntities.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CustomerApi;

[ApiController]
[Route("/api/v1/Owner")]
public class AccountOwnerController(ILogger<AccountOwnerController> logger, IAccountOwnerService service, IServiceProvider serviceProvider) : ControllerBase
{
  private readonly ILogger<AccountOwnerController> _logger = logger;
  private readonly IAccountOwnerService _service = service;
  private readonly IServiceProvider _serviceProvider = serviceProvider;
  

  [HttpPost("List/{Id}")]
  public async Task<ActionResult> CreateAccountOwners(int Id, [FromBody] IEnumerable<CreateAccountOwner> owners)
  {
     var validator = _serviceProvider.GetRequiredService<IValidator<CreateAccountOwner>>();      
     foreach (var owner in owners)
      {
         if(!(await validator.ValidateAsync(owner)).IsValid)
         {
            return BadRequest("One or more owners doesn't have valid client Id");
         }
         
      }
     _logger.LogInformation("Create Account Owners is running...");
     return await _service.AddOwnersToAccount(Id, owners)? Ok() : NoContent();    
  }

  [HttpPost("{Id}")]
  public async Task<ActionResult> CreateAccountOwner(int Id, [FromBody] CreateAccountOwner owner)
  {
     _logger.LogInformation("CreateAccountOwner is running..");
     return await _service.AddOwnersToAccount(Id, new List<CreateAccountOwner>{owner})? Ok() : NoContent();    
  }

  [HttpDelete("Account/{AccountId}/Client/{ClientId}")]
  public async Task<ActionResult> DeleteAccountOwnersByClient(int AccountId, int ClientId)
  {
   _logger.LogInformation("Delete account owner by client Id is running..");
     return await _service.DeleteOwnerToAccount(AccountId, ClientId)? NoContent()  : StatusCode(500, "An unexpected error occurred."); ; 
  }

  [HttpDelete("Account/{AccountId}")]
  public async Task<ActionResult> DeleteAccountOwners(int AccountId)
  {
     _logger.LogInformation("Delete all account Owners");    
     return await _service.DeleteOwnersToAccount(AccountId)? NoContent()  : StatusCode(500, "An unexpected error occurred."); ; 
  }
}
