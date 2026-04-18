#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring the property as nullable.

using System.ComponentModel.DataAnnotations;

namespace UserAuthEntities;

public class OAuthAuthMethod : AuthMethod
{
    [Required]
    public OAuthProvider Provider { get; set; }

    [Required]
    [MaxLength(255)]
    public string ProviderUserId { get; set; }

    [EmailAddress]
    [MaxLength(255)]
    public string? ProviderEmail { get; set; }
}