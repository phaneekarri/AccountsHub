using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserAuthEntities.Configurations;
public class UserOtpConfiguration : IEntityTypeConfiguration<UserOtp>
{
    public void Configure(EntityTypeBuilder<UserOtp> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();        
        builder.Property(x => x.UserIdentifierType)
        .HasConversion(
            x=> x.ToString(),
            x => (UserIdentifierType)Enum.Parse(typeof(UserIdentifierType), x)
        ).HasMaxLength(16);
        builder.Property(x => x.ExpirySpan)
        .HasConversion(
            x=> x.ToString(),
            x => (ExpiryTimeSpan)Enum.Parse(typeof(ExpiryTimeSpan), x)
        ).HasMaxLength(16);
        builder.Property(x => x.Token).IsRequired();       
        builder
        .Ignore(e => e.isActive)
        .Ignore(e => e.IsExpired)
        .Ignore(e => e.ExpiryAt);

    }
}