using LoanEntities.Models.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanEntities.Configurations
{
    public class AccountOwnerTypeConfiguration : IEntityTypeConfiguration<AccountOwnerType>
    {
        public void Configure(EntityTypeBuilder<AccountOwnerType> builder)
        {
            ConfigurationHelpers.Configure(builder);
        }
    }
}
