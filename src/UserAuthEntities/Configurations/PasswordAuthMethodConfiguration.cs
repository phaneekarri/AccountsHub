using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserAuthEntities.Configurations;

public class PasswordAuthMethodConfiguration : IEntityTypeConfiguration<PasswordAuthMethod>
{
    public void Configure(EntityTypeBuilder<PasswordAuthMethod> builder)
    {
        builder.Property(x => x.UserName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.PasswordHash).IsRequired();
        builder.Property(x => x.Salt).IsRequired();
        builder.Property(x => x.PasswordExpiry).HasDefaultValueSql("DATEADD(day, 90, GETUTCDATE())");

        builder.HasIndex(x => x.UserName).IsUnique();
    }
}