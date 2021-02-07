using CustomerEntities.Models;
using CustomerEntities.Models.Types;
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
            if(typeof(TValue) == typeof(string))
            builder.Property(x => x.Value).HasMaxLength(10);

            builder.Property(x => x.Value).IsRequired();
            builder.Property(x => x.ContactType).IsRequired();
            //builder.Property(x => x.Client).IsRequired();
            return builder;
        }
    }
}
