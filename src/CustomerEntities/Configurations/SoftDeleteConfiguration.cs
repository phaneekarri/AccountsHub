using CustomerEntities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerEntities.Configurations
{
    public class SoftDeleteConfiguration : IEntityTypeConfiguration<ISoftDelete>
    {
        public void Configure(EntityTypeBuilder<ISoftDelete> builder)
        {
            builder.Property(p=>p.IsDeleted).HasDefaultValue(false);
            builder.HasQueryFilter(x=>!x.IsDeleted);
        }
    }
}
