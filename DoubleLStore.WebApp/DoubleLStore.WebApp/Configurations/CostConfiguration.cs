using DoubleLStore.WebApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoubleLStore.WebApp.Configurations
{
    public class CostConfiguration : IEntityTypeConfiguration<Costs>
    {
        public void Configure(EntityTypeBuilder<Costs> builder)
        {
            builder.ToTable("Costs");
             builder.HasKey(x => x.Id); 
        }
    }
}
