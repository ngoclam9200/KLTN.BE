
using DoubleLStore.WebApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoubleLStore.WebApp.Configurations
{
    public class VoucherConfiguration : IEntityTypeConfiguration<Vouchers>
    {
        public void Configure(EntityTypeBuilder<Vouchers> builder)
        {
            builder.ToTable("Vouchers");
            builder.HasKey(x=>x.Id);    
        }
    }
}
