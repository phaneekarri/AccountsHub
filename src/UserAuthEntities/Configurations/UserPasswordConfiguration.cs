using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserAuthEntities.InternalUsers;

namespace UserAuthEntities.Configurations;
public class UserPasswordConfiguration : IEntityTypeConfiguration<UserPassWord>
{
    public void Configure(EntityTypeBuilder<UserPassWord> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.ExpirySpan)
        .HasConversion(
            x => x.ToString(), 
            x=> (ExpiryDateSpan)Enum.Parse(typeof(ExpiryDateSpan), x)  
        ).HasMaxLength(15);
        builder.Navigation(c => c.User);
        builder
        .Ignore(x => x.IsExpired)
        .Ignore(x => x.IsDeleted);
        
    }
}
