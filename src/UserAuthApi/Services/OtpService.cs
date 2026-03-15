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

    public async Task<UserOtp> Create(Guid userId, UserIdentifierType otpReceiver)
    {
        var Otp = await GetRecentOtp(userId , otpReceiver);
        if(Otp != null) await Remove(userId, otpReceiver);        
        Otp = GenerateOtp(userId, otpReceiver, otpSettings);
        Context.UserOtps.Add(Otp);        
        await Context.SaveChangesAsync();
        return Otp;
    }

    public async Task<UserOtp?> GetRecentOtp(Guid userId, UserIdentifierType otpReceiver)
    {
        IList<UserOtp>? unexpired = 
                await Context.UserOtps
                .Where(x=> x.UserId == userId && x.UserIdentifierType == otpReceiver)
                .ToListAsync();
        
        return unexpired
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault(x =>  x.isActive); 
    }

    public async Task Remove(Guid userId, UserIdentifierType otpReceiver)
    {
            
        IList<UserOtp>? unexpired = 
                await Context.UserOtps
                .Where(x=> x.UserId == userId && x.UserIdentifierType == otpReceiver)
                .ToListAsync();
        foreach(var otp in unexpired){
            otp.MarkInvalid();
        }
        await Context.SaveChangesAsync();
    }

    public static UserOtp GenerateOtp(Guid userId, UserIdentifierType userIdentifierType, OtpSettings settings)
    => new UserOtp 
    {
        Id = Guid.NewGuid(),
        UserId = userId, 
        UserIdentifierType = userIdentifierType,
        ExpiryIn = settings.ExpiresInSecs, 
        Token = new Random().Next(settings.OtpCodeMin, settings.OtpCodeMax).ToString()
    };

}
