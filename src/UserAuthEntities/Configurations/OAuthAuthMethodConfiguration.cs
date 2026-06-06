using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserAuthEntities.Configurations;

public class OAuthAuthMethodConfiguration : IEntityTypeConfiguration<OAuthAuthMethod>
{
    public void Configure(EntityTypeBuilder<OAuthAuthMethod> builder)
    {
        builder.Property(x => x.Provider).IsRequired().HasConversion<string>().HasMaxLength(20);
        builder.Property(x => x.ProviderUserId).IsRequired().HasMaxLength(255);
        builder.Property(x => x.ProviderEmail).HasMaxLength(255);

        builder.HasIndex(x => new { x.Provider, x.ProviderUserId }).IsUnique();
    }
}