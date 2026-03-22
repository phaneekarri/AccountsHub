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
    public string ProviderEmail { get; set; }
}