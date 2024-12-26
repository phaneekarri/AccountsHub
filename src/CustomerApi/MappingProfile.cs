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

        AccountMapping();

        AccountOwnerMapping();

    }

    private void AccountOwnerMapping()
    {
        CreateMap<GetAccountOwner, AccountOwner>()
            .ForMember(dest => dest.Client, opt => opt.Ignore())
            .ForMember(dest => dest.Account, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(x=> x.ClientId));

        CreateMap<CreateAccountOwner, AccountOwner>()
           .ForMember(dest => dest.Client, opt => opt.MapFrom<AccountOwnerClientMappingResolver>())
           .ForMember(dest => dest.Account, opt => opt.Ignore());           
    }

    private void AccountMapping()
    {
        CreateMap<Account, GetAccount>()
            .ForMember(dest => dest.Owners, opt => opt.MapFrom(src => src.AccountOwners));

        CreateMap<CreateAccount, Account>()
           .ForMember(dest => dest.AccountOwners, opt => opt.Ignore());

        CreateMap<UpdateAccount, Account>()
            .ForMember(dest => dest.AccountOwners, opt => opt.Ignore());
    }

    private void ClientMappings()
    {
        CreateMap<Client, GetClient>();         

        CreateMap<CreateClient, Client>();

        CreateMap<UpdateClient, Client>();
    }
}
