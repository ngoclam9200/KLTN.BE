using DoubleLStore.WebApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoubleLStore.WebApp.Configurations
{
    public class InfoShopConfiguration : IEntityTypeConfiguration<InfoShop>
    {
        public void Configure(EntityTypeBuilder<InfoShop> builder)
        {
            builder.ToTable("InfoShop");
            builder.HasKey(x => x.Id);
        }
    }
}
