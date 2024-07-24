
using InfraEntities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraEntities.Configurations
{
    public class SoftDeleteConfiguration : IEntityTypeConfiguration<ISoftDelete>
    {
        public void Configure(EntityTypeBuilder<ISoftDelete> builder)
        {
            builder.Property(p=>p.IsDeleted);
            builder.HasQueryFilter(x=>!x.IsDeleted);
        }
    }
}
