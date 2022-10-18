using DoubleLStore.WebApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoubleLStore.WebApp.Configurations
{
    public class NotifiConfiguration : IEntityTypeConfiguration<Notifi>
    {
        public void Configure(EntityTypeBuilder<Notifi> builder)
        {
            builder.ToTable("Notifi");
            builder.HasKey(x => x.Id);

        }
    }
}
