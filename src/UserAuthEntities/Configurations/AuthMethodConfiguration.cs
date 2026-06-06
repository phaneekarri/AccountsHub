using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserAuthEntities.Configurations;

public class AuthMethodConfiguration : IEntityTypeConfiguration<AuthMethod>
{
    public void Configure(EntityTypeBuilder<AuthMethod> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.MethodType).HasConversion<string>().HasMaxLength(20);
        builder.Property(x => x.IsEnabled).HasDefaultValue(true);

        builder.HasDiscriminator<AuthMethodType>("MethodType")
            .HasValue<PasswordAuthMethod>(AuthMethodType.Password)
            .HasValue<OAuthAuthMethod>(AuthMethodType.OAuth)
            .HasValue<OtpVerificationAuthMethod>(AuthMethodType.OtpVerification);
    }
}