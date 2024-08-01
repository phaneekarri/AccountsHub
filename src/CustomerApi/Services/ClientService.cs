using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerApi.Dto;
using CustomerApi.Interfaces;
using CustomerEntities;
using CustomerEntities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CustomerApi;

public class ClientService: BaseService<ClientService>, IClientService
{

    public ClientService(ILogger<ClientService> logger, IMapper mapper, CustomerDbContext context) 
    : base(logger, mapper, context)
    {
    }

    public async Task<IEnumerable<GetClient>> GetAll(){        
        return Mapper.Map<IEnumerable<Client> , IEnumerable<GetClient>>(await Context.Clients.ToListAsync());
    }
    
    public async Task<GetClient> GetBy(int Id){
        return Mapper.Map<Client, GetClient>(await Context.Clients.FirstOrDefaultAsync(c => c.Id == Id));
    }

    public async Task<int> Create(CreateClient dto)
    {         
       if(Context.Clients.Any(
             x => x.FirstName == dto.FirstName && x.LastName ==  dto.LastName && x.DOB == dto.DOB)) 
         throw new  DuplicateNameException("client already exists");
       Client client = Mapper.Map<Client>(dto) ;
       Context.Clients.Add(client);
       await Context.SaveChangesAsync();
       return client.Id;
    }
    
    public async Task<bool> Update(int Id, UpdateClient dto)
    {
        Client client = await Context.Clients.FindAsync(Id);
        if(client == null ) 
            throw new KeyNotFoundException("Client not found}"); 
        Mapper.Map(dto, client);
        Context.Clients.Update(client);       
        
        return await Context.SaveChangesAsync() > 0 ; 

    }

    public async Task<bool> Patch(int Id, UpdateClient dto)
    {        
        Client client = await Context.Clients.FindAsync(Id);
        if(client == null ) 
            throw new KeyNotFoundException("Client not found}");  

        if(dto?.FirstName !=null) client.FirstName = dto.FirstName;
        if(dto?.LastName !=null) client.FirstName = dto.LastName;
        if(dto?.DOB != null && dto.DOB != default) client.DOB = dto.DOB;

        Context.Clients.Update(client);       
        
        return await Context.SaveChangesAsync() > 0 ;    
    }

    public async Task<bool> Delete(int Id)
    {   
        var clients = Context.Clients.Where(c=> c.Id == Id);

        if(clients == null || clients.Count()==0)
            throw new KeyNotFoundException("client not found");
        Context.Clients.RemoveRange(clients);
        return await Context.SaveChangesAsync() > 0;        
    }
}
