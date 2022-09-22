using DoubleLStore.WebApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoubleLStore.WebApp.Configurations
{
    public class StatusOrderConfiguration : IEntityTypeConfiguration<StatusOrders>
    {
        public void Configure(EntityTypeBuilder<StatusOrders> builder)
        {
            builder.ToTable("StatusOrders");
            builder.HasKey(x=>x.Id);
            builder.Property(x => x.isDeleted).HasDefaultValue(false);
        }
    }
}

