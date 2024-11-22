using CustomerEntities.Models.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerEntities.Configurations
{
    public class ContactTypeConfiguration : IEntityTypeConfiguration<ContactType>
    {
        public void Configure(EntityTypeBuilder<ContactType> builder)
        {
            ConfigurationHelpers.Configure(builder);
            builder.HasData(
                new ContactType{Id = 1, Description = "Primary"},
                new ContactType{Id = 2, Description = "Secondary"}
            );
        }        
    }    
}
