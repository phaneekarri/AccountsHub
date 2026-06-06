
using Microsoft.EntityFrameworkCore;
using UserAuthApi.Exceptions;
using UserAuthEntities;
using System.Security.Cryptography;

namespace UserAuthApi.Services;

public class UserService : BaseService<UserService, AuthDBContext>, IUserService
{
    public UserService(ILogger<UserService> logger, AuthDBContext context) 
    : base(logger, context)
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
                throw new ConflictException("Multiple users with same identifier exists.", ex);
            }
            else throw;
        }        
    }
    public async Task<User?> Get(Guid id) => await Context.Users.FindAsync(id);
    public async Task<User?> Get(string userName)
    {
        try
        {
            var authMethod = await Context.AuthMethods
                .OfType<PasswordAuthMethod>()
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.UserName == userName);
            return authMethod?.User;
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
    public async Task<User?> EnableMFA(Guid userId, MfaMethod method)
    {
         try
        {
            var user = await Context.Users.FindAsync(userId);
            if(user == null ) throw new KeyNotFoundException("User not found");
            user.MfaEnabled = true;
            user.MfaMethod = method;
            await Context.SaveChangesAsync();
            return user;
        }
        catch(Exception ex)
        {
            Logger.LogError(ex, "Error enabling MFA for user {UserId}", userId);
            throw;
        }
    }
    public async Task<User> Create(User user)
    {
        if(user.Id == default) user.Id = Guid.NewGuid();
        Context.Users.Add(user);
        await Context.SaveChangesAsync();
        return user;
    }
    public async Task<User> Create(User user, string passWordText)
    {
        if(user.Id == default) user.Id = Guid.NewGuid();
        if (string.IsNullOrEmpty(user.UserName))
        {
            throw new ArgumentException("UserName is required for password authentication", nameof(user));
        }
        // Create user
        Context.Users.Add(user);
        // Create password auth method
        var salt = GenerateSalt();
        var passwordMethod = new PasswordAuthMethod
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            MethodType = AuthMethodType.Password,
            CreatedAt = DateTime.UtcNow,
            IsEnabled = true,
            UserName = user.UserName,
            PasswordHash = HashPassword(passWordText, salt),
            Salt = Convert.ToBase64String(salt),
            PasswordExpiry = DateTime.UtcNow.AddDays(90)
        };
        Context.AuthMethods.Add(passwordMethod);
        await Context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> VerifyPassword(string userName, string password)
    {
        var authMethod = await Context.AuthMethods
            .OfType<PasswordAuthMethod>()
            .FirstOrDefaultAsync(x => x.UserName == userName && x.IsEnabled);
        
        if (authMethod == null) return false;
        
        var salt = Convert.FromBase64String(authMethod.Salt);
        var hash = HashPassword(password, salt);
        return hash == authMethod.PasswordHash;
    }

    public async Task<User?> CreateOrLinkOAuthUser(string email, OAuthProvider provider, string providerUserId, string? providerEmail)
    {
        var existingUser = await Context.Users.FirstOrDefaultAsync(x => x.Email == email);
        
        if (existingUser != null)
        {
            // Link OAuth to existing user
            var existingOAuth = await Context.AuthMethods
                .OfType<OAuthAuthMethod>()
                .FirstOrDefaultAsync(x => x.UserId == existingUser.Id && x.Provider == provider);
            
            if (existingOAuth == null)
            {
                var oauthMethod = new OAuthAuthMethod
                {
                    Id = Guid.NewGuid(),
                    UserId = existingUser.Id,
                    MethodType = AuthMethodType.OAuth,
                    CreatedAt = DateTime.UtcNow,
                    IsEnabled = true,
                    Provider = provider,
                    ProviderUserId = providerUserId,
                    ProviderEmail = providerEmail
                };
                Context.AuthMethods.Add(oauthMethod);
                await Context.SaveChangesAsync();
            }
            return existingUser;
        }

        // Create new user with OAuth
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            CreatedAt = DateTime.UtcNow
        };
        Context.Users.Add(newUser);

        var newOAuthMethod = new OAuthAuthMethod
        {
            Id = Guid.NewGuid(),
            UserId = newUser.Id,
            MethodType = AuthMethodType.OAuth,
            CreatedAt = DateTime.UtcNow,
            IsEnabled = true,
            Provider = provider,
            ProviderUserId = providerUserId,
            ProviderEmail = providerEmail
        };
        Context.AuthMethods.Add(newOAuthMethod);
        await Context.SaveChangesAsync();

        return newUser;
    }

    private static byte[] GenerateSalt(int size = 16)
    {
        var salt = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    private static string HashPassword(string password, byte[] salt)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
        {
            byte[] hash = pbkdf2.GetBytes(32);
            return Convert.ToBase64String(hash);
        }
    }
}
