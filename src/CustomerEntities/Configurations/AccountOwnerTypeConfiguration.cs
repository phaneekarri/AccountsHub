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
        }
    }
}
