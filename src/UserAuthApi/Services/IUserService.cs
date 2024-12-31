using UserAuthEntities;
using UserAuthEntities.InternalUsers;

namespace UserAuthApi.Services;

public interface IUserService
{
    Task<User?> Get(Guid id);
    Task<InternalUser?> Get(string userName);
    Task<InternalUser?> EnableMFA(Guid userId);

    Task<User?> Get(UserIdentifierType idType, string id);
    Task<User> Create(User user);
    Task<InternalUser> Create(User user, string passWordText);
}
