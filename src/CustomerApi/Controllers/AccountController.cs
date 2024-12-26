using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApi.Dto;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CustomerApi;
[ApiController]
[Route("/api/v1/[Controller]")]
public class AccountController(ILogger<AccountController> logger, IAccountService service, IServiceProvider serviceProvider ) 
: ControllerBase
{
    private readonly IAccountService _service = service;
    private readonly ILogger<AccountController> _logger = logger;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

   [HttpGet]
   public async Task<ActionResult<IEnumerable<GetAccount>>> GetAll()
   {
      _logger.LogInformation("Account Get All method is running..");
      var result = await _service.GetAll();
      return result.Count() > 0 ? Ok(result) : NotFound("No accounts found");
   }

  [HttpGet("{Id}")]
   public async Task<ActionResult<IEnumerable<GetAccount>>> GetById( int Id)
   {
      _logger.LogInformation("Account Get By Id method is running..");
      var result = await _service.GetBy(Id);
      return result != null ? Ok(result) : NotFound("No accounts found");
   }

   [HttpPost]
   public async Task<ActionResult> Post(CreateAccount account)
   {    
      _logger.LogInformation("Create Account is running..");
      var result = await _service.Create(account);
      return result >0 ? Created("/api/v1/Client", result) : StatusCode(500, "An unexpected error occurred."); 
   }

    [HttpDelete("{Id}")]
    public async Task<ActionResult> Delete(int Id)
    {
        _logger.LogInformation("Client Delete  method is running...");
        var result = await _service.Delete(Id);
        return result ?  NoContent()  : StatusCode(500, "An unexpected error occurred."); ; 
    }   

    
    [HttpPut("{Id}")]
    public async Task<ActionResult<UpdateClient>> Put(int Id, [FromBody] UpdateAccount account)
    {
            
        _logger.LogInformation("Client Put  method is running...");        
        var result = await _service.Update(Id, account);
        return result ?  Accepted("/api/v1/Client")  : StatusCode(500, "An unexpected error occurred."); ; 
    
    }
    [HttpPost("{Id}/owners")]
    public async Task<ActionResult> CreateAccountOwners(int Id, [FromBody] IEnumerable<CreateAccountOwner> owners)
    {
        _logger.LogInformation("Create Account Owners is running...");
        return await _service.AddOwners(Id, owners)? Ok() : NoContent();    
    }

    [HttpPost("{Id}/owner")]
    public async Task<ActionResult> CreateAccountOwner(int Id, [FromBody] CreateAccountOwner owner)
    {
        _logger.LogInformation("CreateAccountOwner is running..");
        return await _service.AddOwners(Id, new List<CreateAccountOwner>{owner})? Ok() : NoContent();    
    }

    [HttpDelete("{Id}/owner/{ClientId}")]
    public async Task<ActionResult> DeleteAccountOwnersByClient(int Id, int ClientId)
    {
    _logger.LogInformation("Delete account owner by client Id is running..");
        return await _service.DeleteOwner(Id, ClientId)? NoContent()  : StatusCode(500, "An unexpected error occurred."); ; 
    }

    [HttpDelete("{Id}/owners")]
    public async Task<ActionResult> DeleteAccountOwners(int Id)
    {
        _logger.LogInformation("Delete all account Owners");    
        return await _service.DeleteOwners(Id)? NoContent()  : StatusCode(500, "An unexpected error occurred."); ; 
    }
}
