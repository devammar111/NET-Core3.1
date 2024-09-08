using Microsoft.EntityFrameworkCore;

namespace DastgyrAPI.Entity
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public virtual DbSet<MobileVerifications> MobileVerifications { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        //public virtual DbSet<SellerOrders> SellerOrders { get; set; }
        public virtual DbSet<OrderItems> OrderItems { get; set; }
        public virtual DbSet<ProductSku> ProductSkus { get; set; }
        public virtual DbSet<ProductSkuUsers> ProductSkuUsers { get; set; }
        public virtual DbSet<SellerChangeRequests> SellerChangeRequests { get; set; }
        public virtual DbSet<ProductSkuUsersReturns> ProductSkuUsersReturns { get; set; }
        public virtual DbSet<WarehouseItems> WarehouseItems { get; set; }
        public virtual DbSet<ExternalSuppliers> ExternalSuppliers { get; set; }
        public virtual DbSet<UserFcmTokens> UserFcmTokens { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ProductSkuUsers>().HasKey(table => new {
                table.UserId,
                table.Id,
                table.SkuId,
            });
            
            builder.Entity<WarehouseItems>().HasKey(table => new {
                table.WarehouseId,
                table.SupplierId,
                table.SkuId,
            });
            builder.Entity<OrderItems>().HasKey(table => new {
                table.OrderId,
                table.SkuId,
            });

            builder.Entity<ProductSkuUsersReturns>().HasKey(table => new {
                table.OrderId,
                table.SkuId,
            });

            builder.Entity<UserFcmTokens>().HasKey(table => new {
                table.UserId
            });
           
        }
    }
}
