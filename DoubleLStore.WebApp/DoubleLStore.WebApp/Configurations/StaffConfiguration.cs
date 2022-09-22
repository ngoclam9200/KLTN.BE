
using DoubleLStore.WebApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoubleLStore.WebApp.Configurations
{
    public class StaffConfiguration : IEntityTypeConfiguration<Staffs>
    {
        public void Configure(EntityTypeBuilder<Staffs> builder)
        {
            builder.ToTable("Staffs");
            builder.HasKey(x=>x.Id);
            builder.Property(x => x.Avatar).HasDefaultValue("");
            builder.Property(x => x.Gender).HasDefaultValue("");
            builder.Property(x => x.isDeleted).HasDefaultValue(false);
        }
    }
}
