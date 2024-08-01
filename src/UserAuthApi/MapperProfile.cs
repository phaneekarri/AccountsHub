using AutoMapper;
using UserAuthEntities;

namespace UserAuthApi;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserLoginModel, User>()        
        .ForMember(dest => dest.Email, 
                        opt => opt.MapFrom(
                            src => src.UserIdentifierType == UserIdentifierType.Email ? 
                                    src.UserIdentifier : default))
        .ForMember(dest => dest.Phone, 
                        opt => opt.MapFrom(
                            src => src.UserIdentifierType == UserIdentifierType.Phone ? 
                                    src.UserIdentifier : default));

        CreateMap<Otp, OtpModel>()
        .ForMember(dest => dest.Otp , opt => opt.MapFrom( src => src.OtpCode))
        .ForMember(dest => dest.IdentifierType, opt => opt.MapFrom(src => src.UserIdentifierType));
    }
}
