namespace MesJolisCotillons.DataAccess.Entities.Configuration
{
    using MesJolisCotillons.DataAccess.Entities.EntityModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("User");
            entity.HasKey(c => c.UserId);

            entity.Property(e => e.UserId).HasColumnName("User_ID");
            entity.Property(e => e.Email).HasColumnName("eMail");
        }
    }
}
