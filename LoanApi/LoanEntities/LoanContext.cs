using LoanEntities.Configurations;
using LoanEntities.Models;
using LoanEntities.Models.Contacts;
using LoanEntities.Models.Types;
using Microsoft.EntityFrameworkCore;

namespace LoanEntities
{
    public class LoanContext : DbContext
    {
        public LoanContext(DbContextOptions options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountOwner> AccountOwners { get; set;}

        public DbSet<Address> Addresses { get; set; }

        public DbSet<ClientEmailContact> ClientEmailContacts { get; set; }
        public DbSet<ClientPhoneContact> ClientPhoneContacts { get; set; }
        public DbSet<ClientAddressContact> ClientAddressContacts { get; set; }

        public DbSet<PrimaryContact> PrimaryContactType { get; set; }
        public DbSet<SecondaryContact> SecondaryContactType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
               .ApplyConfiguration(new AddressConfiguration())
               .ApplyConfiguration(new ClientConfiguration());
               // .ApplyConfiguration(new );
        }
    }
}
