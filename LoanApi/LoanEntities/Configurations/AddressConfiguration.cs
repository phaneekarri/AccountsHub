using LoanEntities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanEntities.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(x => x.AddressLine1)
                .HasMaxLength(150);
            builder.Property(x => x.AddressLine2)
                .HasMaxLength(150);
            builder.Property(x => x.City)
               .HasMaxLength(50);
            builder.Property(x => x.State)
               .HasMaxLength(50);
            builder.Property(x => x.PostalCode)
                .IsUnicode(false)
               .HasMaxLength(15);
        }
    }
}
