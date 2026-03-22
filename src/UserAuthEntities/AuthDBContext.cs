
using Microsoft.EntityFrameworkCore;
using UserAuthEntities.Configurations;

namespace UserAuthEntities;

public class AuthDBContext : DbContext
{
   public AuthDBContext(DbContextOptions options): base(options)
   {

   }
   public DbSet<User> Users  => Set<User>();
   public DbSet<UserOtp> UserOtps => Set<UserOtp>();
   public DbSet<UserAccessToken> UserAccessTokens => Set<UserAccessToken>();
   public DbSet<AuthMethod> AuthMethods => Set<AuthMethod>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder               
        .ApplyConfiguration(new UserConfiguration())
        .ApplyConfiguration(new AuthMethodConfiguration())
        .ApplyConfiguration(new PasswordAuthMethodConfiguration())
        .ApplyConfiguration(new OAuthAuthMethodConfiguration())
        .ApplyConfiguration(new OtpVerificationAuthMethodConfiguration())
        .ApplyConfiguration(new UserOtpConfiguration())
        .ApplyConfiguration(new UserAccessTokenConfiguration());
    }
            


}
