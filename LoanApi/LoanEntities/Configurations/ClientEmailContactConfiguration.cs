using LoanEntities.Models;
using LoanEntities.Models.Contacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanEntities.Configurations
{
    public class ClientEmailContactConfiguration : IEntityTypeConfiguration<ClientEmailContact>
    {
        public void Configure(EntityTypeBuilder<ClientEmailContact> builder)
        {
            ConfigurationHelpers.Configure<ClientEmailContact, string>(builder);
            builder.Property(x => x.Value).HasMaxLength(50);
        }
    }
}
