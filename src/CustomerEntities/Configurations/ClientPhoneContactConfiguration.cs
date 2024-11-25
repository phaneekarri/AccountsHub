using CustomerEntities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerEntities.Configurations
{
    public class ClientPhoneContactConfiguration : IEntityTypeConfiguration<ClientPhoneContact>
    {
        public void Configure(EntityTypeBuilder<ClientPhoneContact> builder)
        {
            ConfigurationHelpers.Configure<ClientPhoneContact, string>(builder);
            builder.Property(x => x.Value)
                   .HasColumnName("Phone")
                   .HasMaxLength(20)
                   .IsRequired();
        }
    }
}
