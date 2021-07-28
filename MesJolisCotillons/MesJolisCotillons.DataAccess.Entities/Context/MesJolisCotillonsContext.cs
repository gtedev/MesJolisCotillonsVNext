namespace MesJolisCotillons.DataAccess.Entities.Context
{
    using MesJolisCotillons.DataAccess.Entities.Configuration;
    using MesJolisCotillons.DataAccess.Entities.EntityModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using System.Linq;

    public class MesJolisCotillonsContext : DbContext, IMesJolisCotillonsContext, ISaveDbContext
    {
        public MesJolisCotillonsContext()
        {
        }

        public MesJolisCotillonsContext(DbContextOptions<MesJolisCotillonsContext> options)
            : base(options)
        {
        }

        public DbSet<Blob> Blob { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<AdminUser> Admin_Users { get; set; }

        public virtual DbSet<CustomerUser> Customer_Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductBlob> ProductBlob { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blob>(new BlobConfiguration().Configure);
            modelBuilder.Entity<User>(new UserConfiguration().Configure);
            modelBuilder.Entity<AdminUser>(new AdminUserConfiguration().Configure);
            modelBuilder.Entity<CustomerUser>(new CustomerUserConfiguration().Configure);
            modelBuilder.Entity<Product>(new ProductConfiguration().Configure);
            modelBuilder.Entity<ProductBlob>(new ProductBlobConfiguration().Configure);
            modelBuilder.Entity<ProductCategory>(new ProductCategoryConfiguration().Configure);
            modelBuilder.Entity<Category>(new CategoryConfiguration().Configure);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return this.Database.BeginTransaction();
        }
    }
}
