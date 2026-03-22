using System.ComponentModel.DataAnnotations;

namespace UserAuthEntities;

public class PasswordAuthMethod : AuthMethod
{
    [Required]
    [MaxLength(100)]
    public string UserName { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public string Salt { get; set; }

    public DateTime? LastPasswordChange { get; set; }

    public DateTime? PasswordExpiry { get; set; }
}