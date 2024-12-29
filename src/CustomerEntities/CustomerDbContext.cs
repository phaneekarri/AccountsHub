
using CustomerEntities.Configurations;
using CustomerEntities.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerEntities
{
    public class CustomerDbContext : DbContext
    {        
        public DbSet<Client> Clients { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountOwner> AccountOwners { get; set;}    

        public DbSet<ClientEmailContact> ClientEmailContacts { get; set; }
        public DbSet<ClientPhoneContact> ClientPhoneContacts { get; set; }
        public DbSet<ClientAddressContact> ClientAddressContacts { get; set; }
        public DbSet<Address> Addresses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder               
               .ApplyConfiguration(new AddressConfiguration())
               .ApplyConfiguration(new AccountConfiguration())
               .ApplyConfiguration(new AccountOwnerConfiguration())
               .ApplyConfiguration(new ClientConfiguration())
               .ApplyConfiguration(new ClientEmailContactConfiguration())
               .ApplyConfiguration(new ClientAddressContactConfiguration())
               .ApplyConfiguration(new ClientPhoneContactConfiguration());
        }
    }
}
