namespace MesJolisCotillons.DataAccess.Entities.Configuration
{
    using MesJolisCotillons.DataAccess.Entities.EntityModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("Category_ID");

            entity.Property(e => e.DisplayName).HasMaxLength(50);

            entity.Property(e => e.EtiquetteFk).HasColumnName("Etiquette_FK");

            entity.Property(e => e.Name).HasMaxLength(50);

            ////entity.HasOne(d => d.EtiquetteFkNavigation)
            ////    .WithMany(p => p.Category)
            ////    .HasForeignKey(d => d.EtiquetteFk)
            ////    .HasConstraintName("FK_Category_ToEtiquette");
        }
    }
}
