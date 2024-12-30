using Infra;
using InfraEntities.Interceptors;
using Microsoft.EntityFrameworkCore;
using UserAuthEntities.Configurations;
using UserAuthEntities.InternalUsers;

namespace UserAuthEntities;

public class AuthDBContext : DbContext
{
   public AuthDBContext(DbContextOptions options): base(options)
   {

   }
   public DbSet<User> Users  => Set<User>();
   public DbSet<UserOtp> UserOtps => Set<UserOtp>();
   public DbSet<UserAccessToken> UserAccessTokens => Set<UserAccessToken>();
   public DbSet<InternalUser> InternalUsers => Set<InternalUser>();
   public DbSet<UserPassWord> UserPassWords => Set<UserPassWord>();
   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder               
        .ApplyConfiguration(new UserConfiguration())
        .ApplyConfiguration(new UserPasswordConfiguration())
        .ApplyConfiguration(new UserOtpConfiguration())
        .ApplyConfiguration(new UserAccessTokenConfiguration())
        .ApplyConfiguration(new InternalUserConfiguration());
    }
            


}
