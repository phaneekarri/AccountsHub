using CustomerApi.Dto;
using CustomerApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CustomerApi.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class ClientController (ILogger<ClientController> logger, IClientService service) : ControllerBase
{
    private readonly ILogger<ClientController> _logger = logger;
    private readonly IClientService _service = service;
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetClient>>> Get()
    {
        _logger.LogInformation("Client Get All method is running...");

        var results =  (await _service.GetAll()).ToList();
        return results.Count > 0 ? Ok(results) : NotFound("No Clients found");
        
    }
    [HttpGet("{Id}")]    
    public async Task<ActionResult<GetClient>> GetById(int Id)
    {
        _logger.LogInformation("Client Get All method is running...");

        var result = await _service.GetBy(Id);

        return result != null ? Ok(result) : NotFound("No Client found");
        
    }
    [HttpPost]
    public async Task<ActionResult<CreateClient>> Post([FromBody] CreateClient client)
    {
        _logger.LogInformation("Client Post  method is running...");
        var result = await _service.Create(client);
        return result >0 ? Created("/api/v1/Client", result) : StatusCode(500, "An unexpected error occurred.");  
    }

    [HttpPut("{Id}")]
    public async Task<ActionResult<UpdateClient>> Put(int Id, [FromBody] UpdateClient client)
    {
            
        _logger.LogInformation("Client Put  method is running...");        
        var result = await _service.Update(Id, client);
        return result ?  Accepted("/api/v1/Client")  : StatusCode(500, "An unexpected error occurred."); ; 
    
    }

     [HttpPatch("{Id}")]
    public async Task<ActionResult<UpdateClient>> Patch( int Id, [FromBody] UpdateClient client)
    {
            
        _logger.LogInformation("Client Put  method is running...");        
        var result = await _service.Patch(Id, client);
        return result ?  Accepted("/api/v1/Client")  : StatusCode(500, "An unexpected error occurred."); ; 
    
    }

    [HttpDelete("{Id}")]
    public async Task<ActionResult> Delete(int Id)
    {
        _logger.LogInformation("Client Delete  method is running...");
        var result = await _service.Delete(Id);
        return result ?  NoContent()  : StatusCode(500, "An unexpected error occurred."); ; 
    }

}
