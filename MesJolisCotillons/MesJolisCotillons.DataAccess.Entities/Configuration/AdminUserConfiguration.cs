namespace MesJolisCotillons.DataAccess.Entities.Configuration
{
    using MesJolisCotillons.DataAccess.Entities.EntityModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AdminUserConfiguration : IEntityTypeConfiguration<AdminUser>
    {
        public void Configure(EntityTypeBuilder<AdminUser> entity)
        {
            entity.HasKey(e => e.UserFk);

            entity.ToTable("Admin_User");

            entity.Property(e => e.UserFk)
                .HasColumnName("User_FK")
                .ValueGeneratedNever();

            entity.HasOne(d => d.User)
                .WithOne(p => p.AdminUser)
                .HasForeignKey<AdminUser>(d => d.UserFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Admin_User_User");
        }
    }
}
