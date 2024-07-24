using InfraEntities;

namespace CustomerEntities;

public class User : AuditEntity, ISoftDelete
{
   public int Id {get; set;}

   public string UserName {get; set;}

   public string Authmode {get; set;}

   public string Otp {get; set;}

   public string otpExpiration {get; set;}

   public string token {get; set;}

   public bool IsDeleted {get; set;}
}
