using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace UserAuthEntities;

public class AuthDBContextFactory : IDesignTimeDbContextFactory<AuthDBContext>
{
    public AuthDBContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("AuthDatabaseConnection")
            ?? throw new InvalidOperationException("Set the AuthDatabaseConnection environment variable for design-time operations.");

        var optionsBuilder = new DbContextOptionsBuilder<AuthDBContext>();
        optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("UserAuthMigrations"));

        return new AuthDBContext(optionsBuilder.Options);
    }
}