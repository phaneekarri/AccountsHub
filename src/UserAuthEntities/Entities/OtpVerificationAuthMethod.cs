#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring the property as nullable.

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