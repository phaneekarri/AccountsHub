using CustomerEntities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerEntities.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.Property(x => x.FirstName)
                    .HasMaxLength(100);

            builder.Property(x => x.LastName)
                    .HasMaxLength(100);
            builder
            .HasMany(c => c.AddressContacts)
            .WithOne(contact => contact.Client)
            .HasForeignKey(contact => contact.ClientId)
            .IsRequired();

            builder
            .Navigation(c => c.AddressContacts)
            .UsePropertyAccessMode(PropertyAccessMode.Field);            

            builder
            .HasMany(c => c.EmailContacts)
            .WithOne(contact => contact.Client)
            .HasForeignKey(contact => contact.ClientId)
            .IsRequired();

            builder
            .Navigation(c => c.EmailContacts)
            .UsePropertyAccessMode(PropertyAccessMode.Field);            


            builder
            .HasMany(c => c.PhoneContacts)
            .WithOne(contact => contact.Client)
            .HasForeignKey(contact => contact.ClientId)
            .IsRequired();

            builder
            .Navigation(c => c.PhoneContacts)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

                   
        
            builder.Ignore(x => x.PrimaryEmail)
                .Ignore(x => x.PrimaryAddress)
                .Ignore(x => x.PrimaryPhone)
                .Ignore(x => x.SecondaryEmail)
                .Ignore(x => x.SecondaryPhone)
                .Ignore(x => x.SecondaryAddresses); 
                           
            builder.HasQueryFilter(x=>!x.IsDeleted);                               
        }
    }
}
