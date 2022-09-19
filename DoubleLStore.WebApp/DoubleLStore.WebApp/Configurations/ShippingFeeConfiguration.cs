
using DoubleLStore.WebApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoubleLStore.WebApp.Configurations
{
    public class ShippingFeeConfiguration : IEntityTypeConfiguration<ShippingFees>
    {
        public void Configure(EntityTypeBuilder<ShippingFees> builder)
        {
            builder.ToTable("ShippingFees");
            builder.HasKey(x=>x.Id);
        }
    }
}
