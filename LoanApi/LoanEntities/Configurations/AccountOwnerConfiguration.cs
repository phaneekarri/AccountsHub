using LoanEntities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanEntities.Configurations
{
    class AccountOwnerConfiguration : IEntityTypeConfiguration<AccountOwner>
    {
        public void Configure(EntityTypeBuilder<AccountOwner> builder)
        {

        }
    }
}
