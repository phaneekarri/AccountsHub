using AutoMapper;
using UserAuthApi.Dto;
using UserAuthEntities;
using UserAuthEntities.InternalUsers;

namespace UserAuthApi;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<InternalUser, InternalUserDto>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User.Id))
        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
        .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone))
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
        .ForMember(dest => dest.IsMFAEnabled, opt => opt.MapFrom(src => src.IsMFAEnabled));
                
        CreateMap<UserLoginModel, User>()        
        .ForMember(dest => dest.Email, 
                        opt => opt.MapFrom(
                            src => src.UserIdentifierType == UserIdentifierType.Email ? 
                                    src.UserIdentifier : default))
        .ForMember(dest => dest.Phone, 
                        opt => opt.MapFrom(
                            src => src.UserIdentifierType == UserIdentifierType.Phone ? 
                                    src.UserIdentifier : default));      

        CreateMap<UserOtp, OtpModel>()
        .ForMember(dest => dest.Otp , opt => opt.MapFrom( src => src.Token))
        .ForMember(dest => dest.IdentifierType, opt => opt.MapFrom(src => src.UserIdentifierType));

        CreateMap<InternalUserRegisterModel, User>()
        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
        .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone));        

    }
}
