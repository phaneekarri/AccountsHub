using UserAuthEntities;

namespace UserAuthApi.Services;

public interface IUserService
{
    Task<User?> Get(Guid id);
    Task<User?> Get(UserIdentifierType idType, string id);
    Task<User?> Get(string userName);
    Task<User> Create(User user);
    Task<User> Create(User user, string passWordText);
    Task<User?> EnableMFA(Guid userId, MfaMethod method);
    Task<bool> VerifyPassword(string userName, string password);
    Task<User?> CreateOrLinkOAuthUser(string email, OAuthProvider provider, string providerUserId, string? providerEmail);
}
