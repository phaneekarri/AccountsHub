using System;
using System.Threading;
using System.Threading.Tasks;
using CustomerEntities.Configurations;
using CustomerEntities.Models;
using CustomerEntities.Models.Contacts;
using CustomerEntities.Models.Types;
using Infra;
using Microsoft.EntityFrameworkCore;

namespace CustomerEntities
{
    public class CustomerDbContext : DbContext
    {
        private readonly string user;
        public CustomerDbContext(DbContextOptions options, IUserResolver userResolver) : base(options) { user = userResolver.Get(); }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountOwner> AccountOwners { get; set;}

        public DbSet<Address> Addresses { get; set; }

        public DbSet<ClientEmailContact> ClientEmailContacts { get; set; }
        public DbSet<ClientPhoneContact> ClientPhoneContacts { get; set; }
        public DbSet<ClientAddressContact> ClientAddressContacts { get; set; }


        public DbSet<ContactType> ContactTypes { get; set; }
        public DbSet<PrimaryContact> PrimaryContactType { get; set; }
        public DbSet<SecondaryContact> SecondaryContactType { get; set; }

        public DbSet<AccountOwnerType> AccountOwnerTypes { get; set; }
        public DbSet<PrimaryAccountOwner> PrimaryAccountOwner { get; set; }
        public DbSet<SecondaryAccountOwner> SecondaryAccountOwner { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder
               .ApplyConfiguration(new AddressConfiguration())
               .ApplyConfiguration(new AccountConfiguration())
               .ApplyConfiguration(new AccountOwnerConfiguration())
               .ApplyConfiguration(new ClientConfiguration())
               .ApplyConfiguration(new AccountOwnerTypeConfiguration())
               .ApplyConfiguration(new ContactTypeConfiguration());
               //.ApplyConfiguration(new ClientEmailContactConfiguration())
               //.ApplyConfiguration(new ClientAddressContactConfiguration())
               //.ApplyConfiguration(new ClientPhoneContactConfiguration());
        }

         public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if(entry.State == EntityState.Deleted && entry.Entity is ISoftDelete)
                {
                    entry.State = EntityState.Modified;
                    ((ISoftDelete)entry.Entity).IsDeleted = true;
                }
                if( entry.Entity is AuditEntity)
                {
                    var audit = (AuditEntity)entry.Entity;
                    if (entry.State == EntityState.Added){

                        audit.CreatedAt = DateTime.Now;
                        audit.CreatedBy = user;
                        audit.UpdatedAt = DateTime.Now;
                        audit.UpdatedBy = user;
                    }
                    if(entry.State == EntityState.Modified) {
                        audit.UpdatedAt = DateTime.Now;
                        audit.UpdatedBy = user;
                    }
                
                }
            }
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if( entry.Entity is AuditEntity)
            {
                var audit = (AuditEntity)entry.Entity;
                if (entry.State == EntityState.Added){

                    audit.CreatedAt = DateTime.Now;
                    audit.UpdatedAt = DateTime.Now;
                }
                if(entry.State == EntityState.Modified) {
                    audit.UpdatedAt = DateTime.Now;
                }
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
    }
}
