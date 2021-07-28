namespace MesJolisCotillons.DataAccess.Entities.Configuration
{
    using MesJolisCotillons.DataAccess.Entities.EntityModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductBlobConfiguration : IEntityTypeConfiguration<ProductBlob>
    {
        public void Configure(EntityTypeBuilder<ProductBlob> entity)
        {
            entity.HasKey(e => new { e.ProductFk, e.BlobFk });

            entity.Property(e => e.ProductFk).HasColumnName("Product_FK");

            entity.Property(e => e.BlobFk).HasColumnName("Blob_FK");

            entity.HasOne(d => d.BlobFkNavigation)
                .WithMany(p => p.ProductBlob)
                .HasForeignKey(d => d.BlobFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductBlob_Blob_FK");

            entity.HasOne(d => d.ProductFkNavigation)
                .WithMany(p => p.ProductBlob)
                .HasForeignKey(d => d.ProductFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductBlob_Product_FK");
        }
    }
}
