using Infra;
using InfraEntities.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace UserAuthEntities;

public class AuthDBContext : DbContext
{
   public AuthDBContext(DbContextOptions options): base(options)
   {

   }
   public DbSet<User> Users {get; set;}
   public DbSet<Otp> Otps {get; set;}

   public DbSet<AuthToken> AuthTokens {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<User>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Otp>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Otp>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Otp>()
            .Ignore(e => e.isActive);
        
        modelBuilder.Entity<AuthToken>()
            .Ignore(e => e.isActive)
            .HasKey(e => e.Id);
        modelBuilder.Entity<AuthToken>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();
             
    }

}
