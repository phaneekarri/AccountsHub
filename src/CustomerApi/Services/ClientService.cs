using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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

public class ClientService: IClientService
{
    private readonly ILogger<ClientService> _logger;
    private readonly CustomerDbContext _context;
    private readonly IMapper _mapper;

    public ClientService(ILogger<ClientService> logger, IMapper mapper, CustomerDbContext context) 
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetClient>> GetAll(){        
        return _mapper.Map<IEnumerable<Client> , IEnumerable<GetClient>>(await _context.Clients.ToListAsync());
    }
    
    public async Task<GetClient> GetBy(int Id){
        return _mapper.Map<Client, GetClient>(await _context.Clients.FirstOrDefaultAsync(c => c.Id == Id));
    }

    public async Task<int> Create(CreateClient dto)
    {         
        if(await _context.Clients.FindAsync(dto.FirstName, dto.LastName, dto.DOB) != null) 
          throw new  DuplicateNameException("client already exists");
        Client client = _mapper.Map<CreateClient, Client>(dto) ;
       _context.Clients.Add(client);
       await _context.SaveChangesAsync();
       return client.Id;
    }
    
    public async Task<bool> Update(int Id, UpdateClient dto)
    {
        Client client = await _context.Clients.FindAsync(Id);
        if(client == null ) 
            throw new KeyNotFoundException("Client not found}"); 
        _mapper.Map(dto, client);
        _context.Clients.Update(client);       
        
        return await _context.SaveChangesAsync() > 0 ; 

    }

    public async Task<bool> Patch(int Id, UpdateClient dto)
    {        
        Client client = await _context.Clients.FindAsync(Id);
        if(client == null ) 
            throw new KeyNotFoundException("Client not found}");  

        if(dto?.FirstName !=null) client.FirstName = dto.FirstName;
        if(dto?.LastName !=null) client.FirstName = dto.LastName;
        if(dto?.DOB != null && dto.DOB != default) client.DOB = dto.DOB;

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
