using DoubleLStore.WebApp.Configurations;
using DoubleLStore.WebApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoubleLStore.WebApp.EF
{
    public class doubleLStoreDbContext : DbContext
    {
        public doubleLStoreDbContext(DbContextOptions options) : base(options)
        {
           
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppConfigConfiguration());
            modelBuilder.ApplyConfiguration(new AddressUserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new StaffConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new VoucherConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new StatusOrderConfiguration());
            modelBuilder.ApplyConfiguration(new ImageProductConfiguration());
            modelBuilder.ApplyConfiguration(new CostVoucherConfiguration());
            modelBuilder.ApplyConfiguration(new SalaryStaffConfiguration());
            modelBuilder.ApplyConfiguration(new CostProductConfiguration());
            modelBuilder.ApplyConfiguration(new ChatUserConfiguration());
            modelBuilder.ApplyConfiguration(new AdminConfiguration());
            modelBuilder.ApplyConfiguration(new InfoShopConfiguration());
            modelBuilder.ApplyConfiguration(new NotifiConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new ProductDetailConfiguration());



            base.OnModelCreating(modelBuilder);    
        }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Staffs> Staffs { get; set; }
        public DbSet<AddressUsers> AddressUsers { get; set; }
        public DbSet<Carts> Carts { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Products> Products { get; set; }
         public DbSet<StatusOrders> StatusOrders { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<Vouchers> Vouchers { get; set; }
        public DbSet<SalaryStaff> SalaryStaffs { get; set; }
        public DbSet<CostProduct> CostProducts { get; set; }
        public DbSet<ImageProduct> ImageProducts { get; set; }
       public DbSet<ChatUser> ChatUsers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<InfoShop> InfoShops { get; set; }
        public DbSet<Notifi> Notifis { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<CostVoucher> CostVouchers { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public object HttpContext { get; internal set; }
    }
}
