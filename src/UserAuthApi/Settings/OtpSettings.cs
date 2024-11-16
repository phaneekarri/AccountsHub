namespace UserAuthApi.Settings;

public class OtpSettings
{
   public int ExpiresInSecs {get; set;}
   public int OtpCodeMin {get; set;} 
   public int OtpCodeMax{get; set;}
}
