using CustomerEntities.Models;
using CustomerEntities.Models.Contacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerEntities.Configurations
{
    public class ClientAddressContactConfiguration : IEntityTypeConfiguration<ClientAddressContact>
    {
        public void Configure(EntityTypeBuilder<ClientAddressContact> builder)
        {
            ConfigurationHelpers.Configure<ClientAddressContact, Address>(builder);
            builder
            .HasOne(x => x.Value) // Value is the navigation property
            .WithMany() 
            .HasForeignKey(x => x.AddressId)
            .IsRequired() ;
        }
    }
}
