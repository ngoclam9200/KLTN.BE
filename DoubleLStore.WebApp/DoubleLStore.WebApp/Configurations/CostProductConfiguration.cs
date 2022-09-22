using DoubleLStore.WebApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoubleLStore.WebApp.Configurations
{
    public class CostProductConfiguration : IEntityTypeConfiguration<CostProduct>
    {
        public void Configure(EntityTypeBuilder<CostProduct> builder)
        {
            builder.ToTable("CostProduct");
            builder.HasKey(x => x.Id);

        }
    }
}
