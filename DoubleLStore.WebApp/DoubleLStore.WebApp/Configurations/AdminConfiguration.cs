using DoubleLStore.WebApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoubleLStore.WebApp.Configurations
{
    public class AdminConfiguration: IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable("Admins");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Avatar).HasDefaultValue("");
            builder.Property(x => x.Gender).HasDefaultValue("");
            builder.Property(x => x.isDeleted).HasDefaultValue(false);
        }
    }
}
