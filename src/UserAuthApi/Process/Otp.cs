using UserAuthApi.Dto;
using UserAuthApi.Services;
using UserAuthEntities;

namespace UserAuthApi.Process;
public class Otp(ILogger<Otp> logger, 
     IOtpService service) : IOtp
{
    private readonly ILogger<Otp> _logger = logger;
    private readonly IOtpService _service = service;
    public async Task<OtpModel> Generate(Guid userId, UserIdentifierType otpReceiver)
    {
        var otpEntity = await _service.Create(userId, otpReceiver);
        var otp = otpEntity.ToOtpModel();
        //Notify(otp);
        return otp;
    }

    public async Task<bool> Verify(Guid userId, UserIdentifierType otpReceiver, string otpCode)
    {
        var latestOtp = await _service.GetRecentOtp(userId, otpReceiver);
        if(latestOtp == null || latestOtp.UserId == default || 
        !string.Equals(otpCode, latestOtp.Token, StringComparison.OrdinalIgnoreCase )) return false;
        await _service.Remove(userId, otpReceiver);
        return true;
    }

    
}