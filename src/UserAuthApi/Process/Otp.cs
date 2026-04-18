using UserAuthApi.Dto;
using UserAuthApi.Services;
using UserAuthEntities;

namespace UserAuthApi.Process;
public class Otp(ILogger<Otp> logger, 
     IOtpService service) : IOtp
{
    private readonly ILogger<Otp> _logger = logger;
    private readonly IOtpService _service = service;
    public async Task<OtpModel> Generate(Guid userId, UserIdentifierType otpReceiver, OtpType otpType = OtpType.Verification)
    {
        var otpEntity = await _service.Create(userId, otpReceiver, otpType);
        var otp = otpEntity.ToOtpModel();
        //Notify(otp);
        return otp;
    }

    public async Task<bool> Verify(Guid userId, UserIdentifierType otpReceiver, string otpCode, OtpType otpType = OtpType.Verification)
    {
        var latestOtp = await _service.GetRecentOtp(userId, otpReceiver, otpType);
        if(latestOtp == null || latestOtp.UserId == default || 
        !string.Equals(otpCode, latestOtp.Token, StringComparison.OrdinalIgnoreCase )) return false;
        await _service.Remove(userId, otpReceiver, otpType);
        return true;
    }

    
}