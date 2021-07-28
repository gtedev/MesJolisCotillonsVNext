namespace MesJolisCotillons.DataAccess.Entities.Configuration
{
    using MesJolisCotillons.DataAccess.Entities.EntityModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BlobConfiguration : IEntityTypeConfiguration<Blob>
    {
        public void Configure(EntityTypeBuilder<Blob> entity)
        {
            entity.Property(e => e.BlobId).HasColumnName("Blob_ID");

            entity.Property(e => e.CreationDateTime).HasColumnType("datetime");

            entity.Property(e => e.FileName).HasMaxLength(200);

            entity.Property(e => e.MimeType).HasMaxLength(50);
        }
    }
}
