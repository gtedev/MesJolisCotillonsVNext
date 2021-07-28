namespace MesJolisCotillons.DataAccess.Entities.Context
{
    using MesJolisCotillons.DataAccess.Entities.EntityModels;
    using Microsoft.EntityFrameworkCore;

    public interface IProductDbContext : IDbContext
    {
        DbSet<Product> Products { get; }

        DbSet<ProductCategory> ProductCategories { get; }

        DbSet<Category> Categories { get; }

        DbSet<ProductBlob> ProductBlob { get; set; }
    }
}
