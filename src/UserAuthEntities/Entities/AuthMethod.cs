using UserAuthEntities.Interfaces;

namespace UserAuthEntities;

public abstract class AuthMethod : ICreated
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AuthMethodType MethodType { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsEnabled { get; set; }
    public User? User { get; set; }
}