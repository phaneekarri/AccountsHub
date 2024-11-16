
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Exceptions;
using UserAuthEntities;

namespace UserAuthApi.Services;

public class UserService : BaseService<UserService, AuthDBContext>, IUserService
{
    public UserService(ILogger<UserService> logger, IMapper mapper, AuthDBContext context) 
    : base(logger, mapper, context)
    {        
    }

    public async Task<User?> Get(UserIdentifierType userIdentifierType, string id)
    {
        return await Context.Users.SingleOrDefaultAsync(x => (userIdentifierType == UserIdentifierType.Email &&  x.Email == id)
         || (userIdentifierType == UserIdentifierType.Phone &&  x.Phone  == id));
    }

    public async Task<User?> Get(Guid id) => await Context.Users.FindAsync(id);

    public async Task<Guid> Create(User user){
        Context.Users.Add(user);
        await Context.SaveChangesAsync();
        return user.Id;
    }
}
