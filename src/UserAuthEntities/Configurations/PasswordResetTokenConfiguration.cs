using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserAuthEntities.Configurations;

public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Token).IsRequired().HasMaxLength(256);
        builder.Property(x => x.ExpirySpan)
            .HasConversion(
                x => x.ToString(),
                x => (ExpiryTimeSpan)Enum.Parse(typeof(ExpiryTimeSpan), x)
            ).HasMaxLength(16);
        builder
            .Ignore(e => e.IsActive)
            .Ignore(e => e.IsExpired)
            .Ignore(e => e.ExpiryAt);
    }
}