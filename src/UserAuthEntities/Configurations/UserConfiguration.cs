using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserAuthEntities.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();        
        builder.Property(x => x.Email).HasMaxLength(255);
        builder.Property(x => x.Phone).HasMaxLength(10);
        builder.Property(x => x.UserName).HasMaxLength(50);
        builder.Property(x => x.MfaEnabled).HasDefaultValue(false);
        builder.Property(x => x.MfaMethod).HasDefaultValue(MfaMethod.SmsOtp);

        builder.HasIndex(x => x.Email).IsUnique().HasFilter("[Email] IS NOT NULL");
        builder.HasIndex(x => x.Phone).IsUnique().HasFilter("[Phone] IS NOT NULL");
        builder.HasIndex(x => x.UserName).IsUnique().HasFilter("[UserName] IS NOT NULL");

        builder.HasMany(x => x.AuthMethods)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}