using UserAuthEntities.Interfaces;

namespace UserAuthEntities.OIDCUsers;

public class OAuthUser : ICreated
{
    public Guid Id {get; set;}
    public Guid UserId {get; set;}
    public OAuthProvider AuthProvider {get; set;}
    public DateTime CreatedAt {get; set;}
}

public enum OAuthProvider{
    Google
}