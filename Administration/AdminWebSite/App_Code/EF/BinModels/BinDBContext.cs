using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;

namespace BinData
{
    public partial class BinDBContext : DbContext
    {
        public BinDBContext()
            : base("name=NW_DataConnectionString")
        {
        }

        public virtual DbSet<Bin> Bins { get; set; }
        public virtual DbSet<BinAdjustment> BinAdjustments { get; set; }
        public virtual DbSet<Crop> Crops { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LocationDistrict> LocationDistricts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bin>()
                .HasKey(e => e.Uid);

            modelBuilder.Entity<Bin>()
                .Property(e => e.Bin1)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Bin")
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Bin_LocationId") { IsUnique = true, Order = 1 }));

            modelBuilder.Entity<Bin>()
                .Property(e => e.LocationId)
                .HasColumnName("Location_Id")
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Bin_LocationId") { IsUnique = true, Order = 2 }));

            modelBuilder.Entity<Bin>()
                .Property(e => e.Uid)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("UID");

            modelBuilder.Entity<Bin>()
                .Property(e => e.BushelsFt)
                .HasColumnType("decimal")
                .HasPrecision(10, 3)
                .HasColumnName("Bushels_Ft");

            modelBuilder.Entity<Bin>()
                .Property(e => e.CropId)
                .HasColumnName("Crop_Id");

            modelBuilder.Entity<Bin>()
                .Property(e => e.TestTimeDate)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .HasColumnType("datetime")
                .HasColumnName("Test_TimeDate");

            modelBuilder.Entity<Bin>()
                .HasRequired(d => d.Crop)
                .WithMany(p => p.Bins)
                .HasForeignKey(d => d.CropId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Bin>()
                .HasRequired(d => d.Location)
                .WithMany(p => p.Bins)
                .HasForeignKey(d => d.LocationId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BinAdjustment>()
                .HasKey(e => e.Uid)
                .ToTable("Bin_Adjustments");

            modelBuilder.Entity<BinAdjustment>()
                .Property(e => e.Uid)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("UID");

            modelBuilder.Entity<BinAdjustment>()
                .Property(e => e.AdjustedDate)
                .HasColumnType("datetime")
                .HasColumnName("Adjusted_Date");

            modelBuilder.Entity<BinAdjustment>()
                .Property(e => e.BinUid)
                .HasColumnName("Bin_UID");

            modelBuilder.Entity<BinAdjustment>()
                .HasRequired(d => d.BinU)
                .WithMany(p => p.BinAdjustments)
                .HasForeignKey(d => d.BinUid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Crop>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Crop>()
                .Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Crops_Description") { IsUnique = true }));

            modelBuilder.Entity<Crop>()
                .Property(e => e.Id)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Crops_Id") { IsUnique = true }));

            modelBuilder.Entity<Crop>()
                .Property(e => e.Uid)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("UID");

            modelBuilder.Entity<Crop>()
                .Property(e => e.Active)
                .HasColumnAnnotation("DefaultValue", true);

            modelBuilder.Entity<Crop>()
                .Property(e => e.ColorIndex)
                .HasColumnName("Color_Index");

            modelBuilder.Entity<Crop>()
                .Property(e => e.PoundPerBushel)
                .HasColumnAnnotation("DefaultValue", 60f)
                .HasColumnName("Pound_Per_Bushel");

            modelBuilder.Entity<Crop>()
                .Property(e => e.SecondaryColorIndex)
                .HasColumnName("Secondary_Color_Index");

            modelBuilder.Entity<Crop>()
                .Property(e => e.UnitOfMeasure)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnAnnotation("DefaultValue", "bu")
                .IsFixedLength()
                .HasColumnName("Unit_Of_Measure");

            modelBuilder.Entity<Crop>()
                .Property(e => e.UseAtElevator)
                .HasColumnName("Use_At_Elevator");

            modelBuilder.Entity<Crop>()
                .Property(e => e.UseAtSeedMill)
                .HasColumnName("Use_At_Seed_Mill");

            modelBuilder.Entity<Location>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Location>()
                .Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Locations_Description") { IsUnique = true }));

            modelBuilder.Entity<Location>()
                .Property(e => e.Id)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Locations_Id") { IsUnique = true }));

           



            modelBuilder.Entity<Location>()
                .Property(e => e.Active)
                .HasColumnAnnotation("DefaultValue", true);

            modelBuilder.Entity<Location>()
                .Property(e => e.DefaultState)
                .HasMaxLength(2)
                .HasColumnAnnotation("DefaultValue", "WA");

            modelBuilder.Entity<Location>()
                .Property(e => e.InProduction)
                .HasColumnAnnotation("DefaultValue", true)
                .HasColumnName("In_Production");

            modelBuilder.Entity<Location>()
                .HasRequired(d => d.DistrictNavigation)
                .WithMany(p => p.Locations)
                .HasForeignKey(d => d.District)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<LocationDistrict>()
               .Property(e => e.Uid)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
               .HasColumnName("UID");

            modelBuilder.Entity<LocationDistrict>()
                .HasKey(e => e.District)
                .ToTable("Location_Districts");

            modelBuilder.Entity<LocationDistrict>()
                .Property(e => e.District)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_Location_Districts") { IsUnique = true }));

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(DbModelBuilder modelBuilder);
    }
}
