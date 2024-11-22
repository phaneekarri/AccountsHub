using CustomerEntities.Models.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerEntities.Configurations
{
    public class AccountOwnerTypeConfiguration : IEntityTypeConfiguration<AccountOwnerType>
    {
        public void Configure(EntityTypeBuilder<AccountOwnerType> builder)
        {
            ConfigurationHelpers.Configure(builder);
            builder.HasData(
                new AccountOwnerType{Id = 1, Description = "Primary"},
                new AccountOwnerType{Id = 2, Description = "Secondary"} 
            );           
        }
    }
}
