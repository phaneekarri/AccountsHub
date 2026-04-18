#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring the property as nullable.

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