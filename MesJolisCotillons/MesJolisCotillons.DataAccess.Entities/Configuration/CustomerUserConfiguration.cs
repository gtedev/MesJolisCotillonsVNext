namespace MesJolisCotillons.DataAccess.Entities.Configuration
{
    using MesJolisCotillons.DataAccess.Entities.EntityModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CustomerUserConfiguration : IEntityTypeConfiguration<CustomerUser>
    {
        public void Configure(EntityTypeBuilder<CustomerUser> entity)
        {
            entity.HasKey(e => e.UserFk);

            entity.ToTable("Customer_User");

            entity.Property(e => e.UserFk)
                .HasColumnName("User_FK")
                .ValueGeneratedNever();

            entity.Property(e => e.XmlData)
                .HasColumnName("xmlData")
                .HasColumnType("xml");

            entity.HasOne(d => d.User)
                .WithOne(p => p.CustomerUser)
                .HasForeignKey<CustomerUser>(d => d.UserFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customer_User_User");
        }
    }
}
