using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserAuthEntities.InternalUsers;

namespace UserAuthEntities.Configurations;
public class InternalUserConfiguration : IEntityTypeConfiguration<InternalUser>
{
    public void Configure(EntityTypeBuilder<InternalUser> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Navigation(x=> x.User);
        builder.HasMany(x => x.PassWords).WithOne(x => x.User)
        .HasForeignKey(x=>x.InternalUserId);
    }
}