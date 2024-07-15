using CustomerEntities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerEntities.Configurations
{
    public class SoftDeleteConfiguration : IEntityTypeConfiguration<ISoftDelete>
    {
        public void Configure(EntityTypeBuilder<ISoftDelete> builder)
        {
            builder.HasQueryFilter(x=>x.DeletedAt.HasValue);
        }
    }
}
