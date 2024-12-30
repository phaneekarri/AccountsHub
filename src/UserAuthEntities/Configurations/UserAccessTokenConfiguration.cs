using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserAuthEntities.Configurations;
public class UserAccessTokenConfiguration : IEntityTypeConfiguration<UserAccessToken>
{
    public void Configure(EntityTypeBuilder<UserAccessToken> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
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