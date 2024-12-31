
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Exceptions;
using UserAuthEntities;
using UserAuthEntities.InternalUsers;

namespace UserAuthApi.Services;

public class UserService : BaseService<UserService, AuthDBContext>, IUserService
{
    public UserService(ILogger<UserService> logger, IMapper mapper, AuthDBContext context) 
    : base(logger, mapper, context)
    {        
    }

    public async Task<User?> Get(UserIdentifierType userIdentifierType, string id)
    {
       
        try
        {
            return await Context.Users.SingleOrDefaultAsync(x => 
            (userIdentifierType == UserIdentifierType.Email &&  x.Email == id)
            || (userIdentifierType == UserIdentifierType.Phone &&  x.Phone  == id));
        }
        catch(Exception ex)
        {
            if(ex is InvalidOperationException && ex.Message == "Sequence contains more than one element.")
            {
                throw new ConflictException("Multiple users with same username exists.", ex);
            }
            else throw;
        }        
    }

    public async Task<InternalUser?> Get(string userName)
    {
        try
        {
            return  await GetQuery()             
            .SingleOrDefaultAsync(x => userName == x.User.UserName);
        }
        catch(Exception ex)
        {
            if(ex is InvalidOperationException && ex.Message == "Sequence contains more than one element.")
            {
                throw new ConflictException("Multiple users with same username exists.", ex);
            }
            else throw;
        }
    }

    public async Task<User?> Get(Guid id) => await Context.Users.FindAsync(id);
    public async Task<InternalUser?> EnableMFA(Guid userId)
    {
         try
        {
            var user =  await GetQuery()                
            .SingleOrDefaultAsync(x => userId == x.UserId);
            if(user == null ) throw new KeyNotFoundException("User not found");
            user.EnableMFA();
            await Context.SaveChangesAsync();
            return user;
        }
        catch(Exception ex)
        {
            if(ex is InvalidOperationException && ex.Message == "Sequence contains more than one element.")
            {
                throw new ConflictException("Multiple users with same user information exists.", ex);
            }
            else throw;
        }
    }

    public async Task<User> Create(User user)
    {
        Context.Users.Add(user);
        await Context.SaveChangesAsync();
        return user;
    }

    public async Task<InternalUser> Create(User user, string passWordText)
    {
        var internalUser = new InternalUser {
                 User = user                  
            };
        internalUser.ResetPassword(passWordText, 90);
        Context.InternalUsers.Add(internalUser);
        await Context.SaveChangesAsync();
        return internalUser;
    }

    private IQueryable<InternalUser> GetQuery() => 
        Context.InternalUsers
            .Include(x => x.User)
            .Include(x=> x.PassWords);
}
