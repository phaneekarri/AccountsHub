using CustomerEntities.Models;
using InfraEntities.ModelType;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerEntities.Configurations
{
    public static class ConfigurationHelpers
    {
       public static EntityTypeBuilder<T> Configure<T>(EntityTypeBuilder<T> builder)
            where T : ModelType
       {
            builder.HasIndex(x => x.Id).IsUnique();
            builder.HasDiscriminator(x => x.Description);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Description).HasMaxLength(50).IsRequired();
            return builder;
       }
        public static EntityTypeBuilder<TEntity> Configure<TEntity, TValue>(EntityTypeBuilder<TEntity> builder)
        where TEntity : ClientContact<TValue>
        {
            
            builder
            .HasOne(c => c.ContactType) // Navigation property
            .WithMany() // ContactType can have many contacts (one-to-many)
            .HasForeignKey(c => c.ContactTypeId) // Foreign key property
            .IsRequired();
            
            builder.HasQueryFilter(cac => !cac.Client.IsDeleted);

            return builder;
        }
    }
}
