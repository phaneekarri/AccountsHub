using AutoMapper;
using Microsoft.Extensions.Options;
using UserAuthApi.Exceptions;
using UserAuthEntities;
using UserAuthApi.Settings;
using UserAuthApi.Dto;
using Microsoft.EntityFrameworkCore;

namespace UserAuthApi.Services;

public class OtpService : BaseService<OtpService, AuthDBContext>, IOtpService
{
    private readonly OtpSettings otpSettings;
    public OtpService(ILogger<OtpService> logger, IMapper mapper, AuthDBContext context, 
     IOptions<OtpSettings> otpOptions)
       :base(logger, mapper, context)
    {
        otpSettings =otpOptions.Value;
    }

    public async Task<OtpModel> Generate(Guid userId, UserIdentifierType otpReceiver)
    {

        var Otp = GenerateOtp(userId, otpReceiver, otpSettings);
        Context.UserOtps.Add(Otp);
        await Context.SaveChangesAsync();
        var model = Mapper.Map<OtpModel>(Otp);
       // await NotifyAsync(model, user.userIdentifier);        
        return model;
    }

    public async Task<bool> Verify(OtpVerficationModel userOtp)
    {
        IList<UserOtp>? unexpired = 
                await Context.UserOtps
                .Where(x=> x.UserId.ToString() == userOtp.UserId && x.UserIdentifierType == userOtp.UserIdentifierType)
                .ToListAsync();
        
        var latestOtp = unexpired
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault(x =>  x.isActive);             
        if(latestOtp == null || latestOtp.UserId == default || 
        !string.Equals(userOtp.Otp, latestOtp.Token, StringComparison.OrdinalIgnoreCase )) 
            throw new OtpException("Otp verification failed");
        foreach(var otp in unexpired){
            otp.MarkInvalid();
        }
        await Context.SaveChangesAsync();
        return true;      
    }

    public static UserOtp GenerateOtp(Guid userId, UserIdentifierType userIdentifierType, OtpSettings settings)
    => new UserOtp 
    {
        UserId = userId, 
        UserIdentifierType = userIdentifierType,
        ExpiryIn = settings.ExpiresInSecs, 
        Token = new Random().Next(settings.OtpCodeMin, settings.OtpCodeMax).ToString()
    };

}
