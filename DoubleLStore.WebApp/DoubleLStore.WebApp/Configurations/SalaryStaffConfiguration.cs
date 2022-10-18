using DoubleLStore.WebApp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoubleLStore.WebApp.Configurations
{
    public class SalaryStaffConfiguration : IEntityTypeConfiguration<SalaryStaff>
    {
        public void Configure(EntityTypeBuilder<SalaryStaff> builder)
        {
            builder.ToTable("SalaryStaff");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.NumberOfWorking).HasDefaultValue(0);
        }
    
    }
}
