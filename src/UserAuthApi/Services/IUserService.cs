using UserAuthEntities;

namespace UserAuthApi.Services;

public interface IUserService
{
    Task<User?> Get(Guid id);
    Task<User?> Get(UserIdentifierType idType, string id);
    Task<Guid> Create(User user);
}
