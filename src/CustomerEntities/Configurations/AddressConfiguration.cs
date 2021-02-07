using CustomerEntities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerEntities.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(x => x.AddressLine1)
                .HasMaxLength(150).IsRequired();
            builder.Property(x => x.AddressLine2)
                .HasMaxLength(150);
            builder.Property(x => x.City)
               .HasMaxLength(50).IsRequired();
            builder.Property(x => x.State)
               .HasMaxLength(50).IsRequired();
            builder.Property(x => x.PostalCode)
                .HasMaxLength(15).IsRequired().IsUnicode(false);

        }
    }
}
