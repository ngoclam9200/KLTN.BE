using DoubleLStore.WebApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoubleLStore.WebApp.Configurations
{
    public class CostVoucherConfiguration : IEntityTypeConfiguration<CostVoucher>
    {
        public void Configure(EntityTypeBuilder<CostVoucher> builder)
        {
            builder.ToTable("CostVouchers");
             builder.HasKey(x => x.Id); 

        }
    }
}
