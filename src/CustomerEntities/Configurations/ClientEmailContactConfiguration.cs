using CustomerEntities.Models;
using CustomerEntities.Models.Contacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerEntities.Configurations
{
    public class ClientEmailContactConfiguration : IEntityTypeConfiguration<ClientEmailContact>
    {
        public void Configure(EntityTypeBuilder<ClientEmailContact> builder)
        {
            ConfigurationHelpers.Configure<ClientEmailContact, string>(builder);
            builder.Property(x => x.Value)
                   .HasColumnName("Email")
                   .HasMaxLength(255)
                   .IsRequired();
        }
    }
}
