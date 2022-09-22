using DoubleLStore.WebApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoubleLStore.WebApp.Configurations
{
    public class AddressUserConfiguration : IEntityTypeConfiguration<AddressUsers>
    {
        public void Configure(EntityTypeBuilder<AddressUsers> builder)
        {
            builder.ToTable("AddressUsers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.isDeleted).HasDefaultValue(false);
        }
    }
}
