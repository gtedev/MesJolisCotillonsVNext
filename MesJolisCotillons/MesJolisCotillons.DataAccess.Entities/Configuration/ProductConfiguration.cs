namespace MesJolisCotillons.DataAccess.Entities.Configuration
{
    using MesJolisCotillons.DataAccess.Entities.EntityModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("Product_ID");

            entity.Property(e => e.DisplayName).HasMaxLength(200);

            entity.Property(e => e.Name).HasMaxLength(200);

            entity.Property(e => e.ProductFlags).HasColumnName("Product_Flags");

            entity.Property(e => e.XmlData)
                .HasColumnName("xmlData")
                .HasColumnType("xml");
        }
    }
}
