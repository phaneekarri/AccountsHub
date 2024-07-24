using InfraEntities;
using InfraEntities.Interfaces;

namespace UserAuthEntities;

public class User:AuditableSoftDeleteEntity, IAuditEntity, ISoftDelete
{
   public int Id {get; set;}

   public string? UserName {get; set;}

   public string? Password {get; set;}

   public string? Authmode {get; set;}

   public string? Otp {get; set;}

   public TimeSpan otpExpiration {get; set;}

   public string? BackupCode {get; set;}


}
