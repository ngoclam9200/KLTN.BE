using DoubleLStore.WebApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoubleLStore.WebApp.Configurations
{
    public class ChatUserConfiguration : IEntityTypeConfiguration<ChatUser>
    {
        public void Configure(EntityTypeBuilder<ChatUser> builder)
        {
            builder.ToTable("ChatUsers");
            builder.HasKey(x => x.ChatId);
            builder.Property(x=>x.isNewMessageAdmin).HasDefaultValue(false);
            builder.Property(x => x.isNewMessageUser).HasDefaultValue(false);
        }
    }
}
