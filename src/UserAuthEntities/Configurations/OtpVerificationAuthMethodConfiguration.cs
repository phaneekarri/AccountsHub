using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserAuthEntities.Configurations;

public class OtpVerificationAuthMethodConfiguration : IEntityTypeConfiguration<OtpVerificationAuthMethod>
{
    public void Configure(EntityTypeBuilder<OtpVerificationAuthMethod> builder)
    {
        builder.Property(x => x.VerificationType).IsRequired().HasConversion<string>().HasMaxLength(10);
        builder.Property(x => x.Identifier).IsRequired().HasMaxLength(255);

        builder.HasIndex(x => new { x.UserId, x.VerificationType }).IsUnique();
        builder.HasIndex(x => x.Identifier).IsUnique();
    }
}