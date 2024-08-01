using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserAuthApi.Exceptions;
using UserAuthEntities;

namespace UserAuthApi;

public class OtpService : BaseService<OtpService, AuthDBContext>, IOtpService
{
    private readonly OtpSettings otpSettings;
    public OtpService(ILogger<OtpService> logger, IMapper mapper, AuthDBContext context, 
     IOptions<OtpSettings> otpOptions)
       :base(logger, mapper, context)
    {
        otpSettings =otpOptions.Value;
    }

    public async Task<OtpModel> Generate(UserLoginModel user, Guid userId)
    {
        if(user == null) throw new ArgumentNullException("No user is provided to generate otp");
        var Otp = GenerateOtp(userId, user.UserIdentifierType, otpSettings);
        Context.Otps.Add(Otp);
        await Context.SaveChangesAsync();
        var model = Mapper.Map<OtpModel>(Otp);
       // await NotifyAsync(model, user.userIdentifier);        
        return model;
    }

    public async Task<User> Verify(OtpVerficationModel userOtp)
    {
        Otp? otp = Context.Otps.Include(x=> x.User).Where(x=> userOtp.Otp == x.OtpCode 
                && x.UserId.ToString() == userOtp.UserId && x.UserIdentifierType == (int)userOtp.UserIdentifierType)
                .AsEnumerable()
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault(x =>x.isActive);             
        if(otp?.User == null) throw new OtpException("Invalid Otp");
        otp.MarkUsed();
        await Context.SaveChangesAsync();
        return otp.User;      
    }

    private static Otp GenerateOtp(Guid userId, UserIdentifierType userIdentifierType, OtpSettings settings)
    => new Otp 
    {
        UserId = userId, UserIdentifierType = (int)userIdentifierType,
        ExpiresInSecs = settings.ExpiresInSecs, 
        OtpCode = new Random().Next(settings.OtpCodeMin, settings.OtpCodeMax).ToString()
    };

}
