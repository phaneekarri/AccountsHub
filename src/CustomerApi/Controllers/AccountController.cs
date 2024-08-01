using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CustomerApi;
[ApiController]
[Route("/api/v1/[Controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _service;
    private readonly ILogger<AccountController> _logger;
   public AccountController(ILogger<AccountController> logger, IAccountService service )
   {
     _service = service;
     _logger = logger;
   }

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

}
