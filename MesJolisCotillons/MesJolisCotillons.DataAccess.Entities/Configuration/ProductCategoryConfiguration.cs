namespace MesJolisCotillons.DataAccess.Entities.Configuration
{
    using MesJolisCotillons.DataAccess.Entities.EntityModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> entity)
        {
            entity.HasKey(e => new { e.ProductFk, e.CategoryFk });

            entity.ToTable("Product_Category");

            entity.Property(e => e.ProductFk).HasColumnName("Product_FK");

            entity.Property(e => e.CategoryFk).HasColumnName("Category_FK");

            entity.HasOne(d => d.CategoryFkNavigation)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(d => d.CategoryFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Category_Category_FK");

            entity.HasOne(d => d.ProductFkNavigation)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(d => d.ProductFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Category_Product_FK");
        }
    }
}
