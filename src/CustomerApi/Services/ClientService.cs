using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CustomerApi.Dto;
using CustomerApi.Interfaces;
using CustomerEntities;
using CustomerEntities.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerApi;

public class ClientService(CustomerDbContext context)
 : IClientService
{
    private readonly CustomerDbContext _context = context;

    public async Task<IEnumerable<GetClient>> GetAll(){        
        return (await _context.Clients.ToListAsync())
            .Select(c => c.ToGetClient());
    }
    
    public async Task<GetClient> GetBy(int Id){
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == Id);
        return client?.ToGetClient();
    }

    public async Task<int> Create(CreateClient dto)
    {         
       if(_context.Clients.Any(
             x => x.FirstName == dto.FirstName && x.LastName ==  dto.LastName && x.DOB == dto.DOB)) 
         throw new  DuplicateNameException("client already exists");
       Client client = dto.ToClient();
       _context.Clients.Add(client);
       await _context.SaveChangesAsync();
       return client.Id;
    }
    
    public async Task<bool> Update(int Id, UpdateClient dto)
    {
        Client client = await _context.Clients.FindAsync(Id);
        if(client == null ) 
            throw new KeyNotFoundException("Client not found}"); 
        if(_context.Clients.Any(
             x => x.Id != Id 
             && x.FirstName == dto.FirstName
              && x.LastName ==  dto.LastName 
              && x.DOB == dto.DOB)) 
         throw new  DuplicateNameException("client already exists");
        client.UpdateFrom(dto);
        _context.Clients.Update(client);       
        
        return await _context.SaveChangesAsync() > 0 ; 

    }

    public async Task<bool> Patch(int Id, PatchClient dto)
    {        
        Client client = await _context.Clients.FindAsync(Id);
        if(client == null ) 
            throw new KeyNotFoundException("Client not found}");  
        
        if(dto?.FirstName !=null
         && _context.Clients.Any(
             x => x.Id != Id 
             && x.FirstName == dto.FirstName
              && x.LastName ==  (dto.LastName ?? client.LastName) 
              && x.DOB == (dto.DOB ?? client.DOB))) 
         throw new  DuplicateNameException("client already exists");
       
        if(dto?.FirstName !=null) client.FirstName = dto.FirstName;
        if(dto?.LastName !=null) client.LastName = dto.LastName;
        if(dto?.DOB != null && dto.DOB != default) client.DOB = dto.DOB.Value;

        _context.Clients.Update(client);       
        
        return await _context.SaveChangesAsync() > 0 ;    
    }

    public async Task<bool> Delete(int Id)
    {   
        var clients = _context.Clients.Where(c=> c.Id == Id);
        if(clients == null || clients.Count()==0)
            throw new KeyNotFoundException("client not found");
        _context.Clients.RemoveRange(clients);
        return await _context.SaveChangesAsync() > 0;        
    }
}
