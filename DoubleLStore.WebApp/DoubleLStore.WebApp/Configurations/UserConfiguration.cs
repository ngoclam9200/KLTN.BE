
using DoubleLStore.WebApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoubleLStore.WebApp.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x=>x.Id);
            builder.Property(x => x.Avatar).HasDefaultValue("");
            builder.Property(x => x.Gender).HasDefaultValue("");
            builder.Property(x => x.isDeleted).HasDefaultValue(false);
            builder.Property(x => x.isVerify).HasDefaultValue(false);


        }
    }
}
