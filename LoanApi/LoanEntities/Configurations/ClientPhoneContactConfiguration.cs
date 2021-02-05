using LoanEntities.Models.Contacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanEntities.Configurations
{
    public class ClientPhoneContactConfiguration : IEntityTypeConfiguration<ClientPhoneContact>
    {
        public void Configure(EntityTypeBuilder<ClientPhoneContact> builder)
        {
            ConfigurationHelpers.Configure<ClientPhoneContact, string>(builder);
        }
    }
}
