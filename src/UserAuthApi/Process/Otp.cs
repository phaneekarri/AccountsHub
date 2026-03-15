using AutoMapper;
using UserAuthApi.Dto;
using UserAuthApi.Services;
using UserAuthEntities;

namespace UserAuthApi.Process;
public class Otp(ILogger<Otp> logger , IMapper mapper, 
     IOtpService service) : IOtp
{
    private readonly ILogger<Otp> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IOtpService _service = service;
    public async Task<OtpModel> Generate(Guid userId, UserIdentifierType otpReceiver)
    {
        var otp =  _mapper.Map<OtpModel>(await _service.Create(userId, otpReceiver));
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