using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserAuthEntities.OIDCUsers;

public class OAuthUserConfiguration
{
    public void Configure(EntityTypeBuilder<OAuthUser> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Navigation(x=> x.User);
    }
}