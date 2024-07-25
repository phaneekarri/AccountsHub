using System;
using AutoMapper;
using CustomerApi.Dto;
using CustomerEntities.Models;

namespace CustomerApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ClientMappings();

    }

    private void ClientMappings()
    {
        CreateMap<GetClient, Client>();

        CreateMap<CreateClient, Client>();

        CreateMap<UpdateClient, Client>();
    }
}
