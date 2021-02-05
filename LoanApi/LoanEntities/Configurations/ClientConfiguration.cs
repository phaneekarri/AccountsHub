using LoanEntities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanEntities.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.Property(x => x.FirstName)
                    .HasMaxLength(100);

            builder.Property(x => x.LastName)
                    .HasMaxLength(100);

            builder.Ignore(x => x.PrimaryEmail)
                .Ignore(x => x.PrimaryAddress)
                .Ignore(x => x.PrimaryPhone)
                .Ignore(x => x.SecondaryEmail)
                .Ignore(x => x.SecondaryPhone)
                .Ignore(x => x.SecondaryAddresses);                       
        }
    }
}
