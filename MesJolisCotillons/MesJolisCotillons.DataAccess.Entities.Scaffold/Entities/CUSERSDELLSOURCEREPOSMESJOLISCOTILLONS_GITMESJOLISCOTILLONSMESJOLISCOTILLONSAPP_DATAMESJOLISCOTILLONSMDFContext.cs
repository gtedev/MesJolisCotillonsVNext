using Microsoft.EntityFrameworkCore;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class CUSERSDELLSOURCEREPOSMESJOLISCOTILLONS_GITMESJOLISCOTILLONSMESJOLISCOTILLONSAPP_DATAMESJOLISCOTILLONSMDFContext : DbContext
    {
        public CUSERSDELLSOURCEREPOSMESJOLISCOTILLONS_GITMESJOLISCOTILLONSMESJOLISCOTILLONSAPP_DATAMESJOLISCOTILLONSMDFContext()
        {
        }

        public CUSERSDELLSOURCEREPOSMESJOLISCOTILLONS_GITMESJOLISCOTILLONSMESJOLISCOTILLONSAPP_DATAMESJOLISCOTILLONSMDFContext(DbContextOptions<CUSERSDELLSOURCEREPOSMESJOLISCOTILLONS_GITMESJOLISCOTILLONSMESJOLISCOTILLONSAPP_DATAMESJOLISCOTILLONSMDFContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<AdminUser> AdminUser { get; set; }
        public virtual DbSet<Blob> Blob { get; set; }
        public virtual DbSet<CarouselImage> CarouselImage { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Command> Command { get; set; }
        public virtual DbSet<CommandAddress> CommandAddress { get; set; }
        public virtual DbSet<CommandDocument> CommandDocument { get; set; }
        public virtual DbSet<CommandHistory> CommandHistory { get; set; }
        public virtual DbSet<CommandProduct> CommandProduct { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<CustomerAddress> CustomerAddress { get; set; }
        public virtual DbSet<CustomerUser> CustomerUser { get; set; }
        public virtual DbSet<DesignConfig> DesignConfig { get; set; }
        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<Etiquette> Etiquette { get; set; }
        public virtual DbSet<KeyWord> KeyWord { get; set; }
        public virtual DbSet<LaPosteColissimo> LaPosteColissimo { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductBlob> ProductBlob { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<ProductKeyWord> ProductKeyWord { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source = (LocalDB)\\MSSQLLocalDB;DataBase=C:\\USERS\\DELL\\SOURCE\\REPOS\\MESJOLISCOTILLONS_GIT\\MESJOLISCOTILLONS\\MESJOLISCOTILLONS\\APP_DATA\\MESJOLISCOTILLONS.MDF;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.AddressId).HasColumnName("Address_ID");

                entity.Property(e => e.Address1)
                    .HasColumnName("Address")
                    .HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.CountryFk).HasColumnName("Country_FK");

                entity.Property(e => e.ZipCode)
                    .HasColumnName("Zip_Code")
                    .HasMaxLength(50);

                entity.HasOne(d => d.CountryFkNavigation)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.CountryFk)
                    .HasConstraintName("FK_Address_Country_FK");
            });

            modelBuilder.Entity<AdminUser>(entity =>
            {
                entity.HasKey(e => e.UserFk);

                entity.ToTable("Admin_User");

                entity.Property(e => e.UserFk)
                    .HasColumnName("User_FK")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.UserFkNavigation)
                    .WithOne(p => p.AdminUser)
                    .HasForeignKey<AdminUser>(d => d.UserFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Admin_User_User");
            });

            modelBuilder.Entity<Blob>(entity =>
            {
                entity.Property(e => e.BlobId).HasColumnName("Blob_ID");

                entity.Property(e => e.CreationDateTime).HasColumnType("datetime");

                entity.Property(e => e.FileName).HasMaxLength(200);

                entity.Property(e => e.MimeType).HasMaxLength(50);
            });

            modelBuilder.Entity<CarouselImage>(entity =>
            {
                entity.Property(e => e.CarouselImageId).HasColumnName("CarouselImage_ID");

                entity.Property(e => e.BlobFk).HasColumnName("Blob_FK");

                entity.Property(e => e.CarouselImageName)
                    .HasColumnName("CarouselImage_Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.BlobFkNavigation)
                    .WithMany(p => p.CarouselImage)
                    .HasForeignKey(d => d.BlobFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CarrouselImage_ToBlob");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("Category_ID");

                entity.Property(e => e.DisplayName).HasMaxLength(50);

                entity.Property(e => e.EtiquetteFk).HasColumnName("Etiquette_FK");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.EtiquetteFkNavigation)
                    .WithMany(p => p.Category)
                    .HasForeignKey(d => d.EtiquetteFk)
                    .HasConstraintName("FK_Category_ToEtiquette");
            });

            modelBuilder.Entity<Command>(entity =>
            {
                entity.Property(e => e.CommandId).HasColumnName("Command_ID");

                entity.Property(e => e.CommandStatus).HasColumnName("Command_Status");

                entity.Property(e => e.CreationDateTime).HasColumnType("datetime");

                entity.Property(e => e.CustomerUserFk).HasColumnName("Customer_User_FK");

                entity.Property(e => e.XmlData)
                    .HasColumnName("xmlData")
                    .HasColumnType("xml");

                entity.HasOne(d => d.CustomerUserFkNavigation)
                    .WithMany(p => p.Command)
                    .HasForeignKey(d => d.CustomerUserFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Command_Customer_User");
            });

            modelBuilder.Entity<CommandAddress>(entity =>
            {
                entity.HasKey(e => e.AddressFk);

                entity.Property(e => e.AddressFk)
                    .HasColumnName("Address_FK")
                    .ValueGeneratedNever();

                entity.Property(e => e.CommandFk).HasColumnName("Command_FK");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.AddressFkNavigation)
                    .WithOne(p => p.CommandAddress)
                    .HasForeignKey<CommandAddress>(d => d.AddressFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommandAddress_ToAddress");

                entity.HasOne(d => d.CommandFkNavigation)
                    .WithMany(p => p.CommandAddress)
                    .HasForeignKey(d => d.CommandFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommandAddress_ToCommand");
            });

            modelBuilder.Entity<CommandDocument>(entity =>
            {
                entity.HasKey(e => e.DocumentFk);

                entity.ToTable("Command_Document");

                entity.Property(e => e.DocumentFk)
                    .HasColumnName("Document_FK")
                    .ValueGeneratedNever();

                entity.Property(e => e.CommandFk).HasColumnName("Command_FK");

                entity.HasOne(d => d.CommandFkNavigation)
                    .WithMany(p => p.CommandDocument)
                    .HasForeignKey(d => d.CommandFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Command_Document_ToCommand");

                entity.HasOne(d => d.DocumentFkNavigation)
                    .WithOne(p => p.CommandDocument)
                    .HasForeignKey<CommandDocument>(d => d.DocumentFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Command_Document_ToDocument");
            });

            modelBuilder.Entity<CommandHistory>(entity =>
            {
                entity.ToTable("Command_History");

                entity.Property(e => e.CommandHistoryId).HasColumnName("Command_History_ID");

                entity.Property(e => e.CommandFk).HasColumnName("Command_FK");

                entity.Property(e => e.CommandHistoryAction).HasColumnName("Command_History_Action");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.XmlData)
                    .HasColumnName("xmlData")
                    .HasColumnType("xml");

                entity.HasOne(d => d.CommandFkNavigation)
                    .WithMany(p => p.CommandHistory)
                    .HasForeignKey(d => d.CommandFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Command_History_ToCommand");
            });

            modelBuilder.Entity<CommandProduct>(entity =>
            {
                entity.Property(e => e.CommandProductId).HasColumnName("CommandProduct_ID");

                entity.Property(e => e.CommandFk).HasColumnName("Command_FK");

                entity.Property(e => e.ProductFk).HasColumnName("Product_FK");

                entity.Property(e => e.XmlData)
                    .HasColumnName("xmlData")
                    .HasColumnType("xml");

                entity.HasOne(d => d.CommandFkNavigation)
                    .WithMany(p => p.CommandProduct)
                    .HasForeignKey(d => d.CommandFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommandProduct_Command_FK");

                entity.HasOne(d => d.ProductFkNavigation)
                    .WithMany(p => p.CommandProduct)
                    .HasForeignKey(d => d.ProductFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommandProduct_Product_FK");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.CountryId).HasColumnName("Country_ID");

                entity.Property(e => e.IsoCode)
                    .HasColumnName("ISO_Code")
                    .HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<CustomerAddress>(entity =>
            {
                entity.HasKey(e => new { e.CustomerUserFk, e.AddressFk });

                entity.Property(e => e.CustomerUserFk).HasColumnName("Customer_User_FK");

                entity.Property(e => e.AddressFk).HasColumnName("Address_FK");

                entity.HasOne(d => d.AddressFkNavigation)
                    .WithMany(p => p.CustomerAddress)
                    .HasForeignKey(d => d.AddressFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerAddress_Address_FK");

                entity.HasOne(d => d.CustomerUserFkNavigation)
                    .WithMany(p => p.CustomerAddress)
                    .HasForeignKey(d => d.CustomerUserFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerAddress_Customer_User");
            });

            modelBuilder.Entity<CustomerUser>(entity =>
            {
                entity.HasKey(e => e.UserFk);

                entity.ToTable("Customer_User");

                entity.Property(e => e.UserFk)
                    .HasColumnName("User_FK")
                    .ValueGeneratedNever();

                entity.Property(e => e.XmlData)
                    .HasColumnName("xmlData")
                    .HasColumnType("xml");

                entity.HasOne(d => d.UserFkNavigation)
                    .WithOne(p => p.CustomerUser)
                    .HasForeignKey<CustomerUser>(d => d.UserFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_User_User");
            });

            modelBuilder.Entity<DesignConfig>(entity =>
            {
                entity.ToTable("Design_Config");

                entity.Property(e => e.DesignConfigId).HasColumnName("Design_Config_ID");

                entity.Property(e => e.CreationDatetime).HasColumnType("datetime");

                entity.Property(e => e.LastUpdateDatetime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.XmlData)
                    .HasColumnName("xmlData")
                    .HasColumnType("xml");
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.Property(e => e.DocumentId).HasColumnName("Document_ID");

                entity.Property(e => e.BlobFk).HasColumnName("Blob_FK");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BlobFkNavigation)
                    .WithMany(p => p.Document)
                    .HasForeignKey(d => d.BlobFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Document_Blob");
            });

            modelBuilder.Entity<Etiquette>(entity =>
            {
                entity.Property(e => e.EtiquetteId).HasColumnName("Etiquette_ID");

                entity.Property(e => e.BlobFk).HasColumnName("Blob_FK");

                entity.Property(e => e.EtiquetteName)
                    .HasColumnName("Etiquette_Name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.BlobFkNavigation)
                    .WithMany(p => p.Etiquette)
                    .HasForeignKey(d => d.BlobFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Etiquette_ToBlob");
            });

            modelBuilder.Entity<KeyWord>(entity =>
            {
                entity.Property(e => e.KeyWordId).HasColumnName("KeyWord_ID");

                entity.Property(e => e.Value)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LaPosteColissimo>(entity =>
            {
                entity.HasKey(e => e.WeightId);

                entity.Property(e => e.WeightId)
                    .HasColumnName("Weight_ID")
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("Product_ID");

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.MainBlobFk).HasColumnName("Main_Blob_FK");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.ProductFlags).HasColumnName("Product_Flags");

                entity.Property(e => e.XmlData)
                    .HasColumnName("xmlData")
                    .HasColumnType("xml");

                entity.HasOne(d => d.MainBlobFkNavigation)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.MainBlobFk)
                    .HasConstraintName("Product_Blob_FK");
            });

            modelBuilder.Entity<ProductBlob>(entity =>
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
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => new { e.ProductFk, e.CategoryFk });

                entity.ToTable("Product_Category");

                entity.Property(e => e.ProductFk).HasColumnName("Product_FK");

                entity.Property(e => e.CategoryFk).HasColumnName("Category_FK");

                entity.HasOne(d => d.CategoryFkNavigation)
                    .WithMany(p => p.ProductCategory)
                    .HasForeignKey(d => d.CategoryFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Category_Category_FK");

                entity.HasOne(d => d.ProductFkNavigation)
                    .WithMany(p => p.ProductCategory)
                    .HasForeignKey(d => d.ProductFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Category_Product_FK");
            });

            modelBuilder.Entity<ProductKeyWord>(entity =>
            {
                entity.HasKey(e => new { e.ProductFk, e.KeyWordFk });

                entity.ToTable("Product_KeyWord");

                entity.Property(e => e.ProductFk).HasColumnName("Product_FK");

                entity.Property(e => e.KeyWordFk).HasColumnName("KeyWord_FK");

                entity.HasOne(d => d.KeyWordFkNavigation)
                    .WithMany(p => p.ProductKeyWord)
                    .HasForeignKey(d => d.KeyWordFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_KeyWord_Blob_FK");

                entity.HasOne(d => d.ProductFkNavigation)
                    .WithMany(p => p.ProductKeyWord)
                    .HasForeignKey(d => d.ProductFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_KeyWord_KeyWord_FK");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.EMail)
                    .HasColumnName("eMail")
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });
        }
    }
}
