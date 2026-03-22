using System.ComponentModel.DataAnnotations;

namespace UserAuthEntities;

public class OtpVerificationAuthMethod : AuthMethod
{
    [Required]
    public VerificationMethod VerificationType { get; set; }

    [Required]
    [MaxLength(255)]
    public string Identifier { get; set; }  // Email or phone
}