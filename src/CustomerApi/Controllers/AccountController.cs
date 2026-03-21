using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CustomerApi;
[ApiController]
[Route("/api/v1/[Controller]")]
public class AccountController(
    ILogger<AccountController> logger, 
    IAccountService service) 
: ControllerBase
{
   [HttpGet]
   public async Task<ActionResult<IEnumerable<GetAccount>>> GetAll()
   {
      logger.LogInformation("Account Get All method is running..");
      var result = await service.GetAll();
      return result.Count() > 0 ? Ok(result) : NotFound("No accounts found");
   }

  [HttpGet("{Id}")]
   public async Task<ActionResult<IEnumerable<GetAccount>>> GetById( int Id)
   {
      logger.LogInformation("Account Get By Id method is running..");
      var result = await service.GetBy(Id);
      return result != null 
      ? Ok(result) 
      : NotFound("No accounts found");
   }

   [HttpPost]
   public async Task<ActionResult> Post(CreateAccount account)
   {    
      logger.LogInformation("Create Account is running..");
      var result = await service.Create(account);
      return result >0 
      ? Created("/api/v1/Client", result) 
      : StatusCode(500, "An unexpected error occurred."); 
   }

    [HttpDelete("{Id}")]
    public async Task<ActionResult> Delete(int Id)
    {
        logger.LogInformation("Client Delete  method is running...");
        var result = await service.Delete(Id);
        return result 
        ?  NoContent()  
        : StatusCode(500, "An unexpected error occurred."); ; 
    }   

    
    [HttpPut("{Id}")]
    public async Task<ActionResult<UpdateClient>> Put(int Id, [FromBody] UpdateAccount account)
    {
            
        logger.LogInformation("Client Put  method is running...");        
        var result = await service.Update(Id, account);
        return result 
        ?  Accepted("/api/v1/Client")  
        : StatusCode(500, "An unexpected error occurred.");     
    }


}
