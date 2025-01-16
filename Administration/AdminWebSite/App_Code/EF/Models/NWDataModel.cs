using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;

public partial class NWDataModel : DbContext
{
    public NWDataModel()
        : base("name=NW_DataConnectionString")
    {
    }

    public virtual DbSet<Crop> Crops { get; set; }
    public virtual DbSet<Landlord> Landlords { get; set; }
    public virtual DbSet<Producer> Producers { get; set; }
    public virtual DbSet<CropProducerFilter> CropProducerFilters { get; set; }
    public virtual DbSet<LoadBinProtein> LoadBinProteins { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Crop>()
            .Property(e => e.Unit_Of_Measure)
            .IsFixedLength()
            .IsUnicode(false);

        modelBuilder.Entity<Landlord>()
            .HasKey(e => e.Uid);

        modelBuilder.Entity<Landlord>()
            .Property(e => e.Uid)
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
            .HasColumnName("UID");

        modelBuilder.Entity<Landlord>()
            .Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Landloards") { IsUnique = true }));

        modelBuilder.Entity<LoadBinProtein>()
            .HasKey(e => e.Uid);

        modelBuilder.Entity<LoadBinProtein>()
            .ToTable("LoadBinProteins");

        modelBuilder.Entity<LoadBinProtein>()
            .Property(e => e.Bin)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("bin");

        modelBuilder.Entity<LoadBinProtein>()
            .Property(e => e.Crop)
            .IsRequired()
            .HasMaxLength(255);

        modelBuilder.Entity<LoadBinProtein>()
            .Property(e => e.LoadId)
            .HasColumnName("Load_Id");

        modelBuilder.Entity<LoadBinProtein>()
            .Property(e => e.Location)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<LoadBinProtein>()
            .Property(e => e.LocationId)
            .HasColumnName("Location_Id");

        modelBuilder.Entity<LoadBinProtein>()
            .Property(e => e.TimeOut)
            .HasColumnType("datetime")
            .HasColumnName("Time_Out");

        modelBuilder.Entity<LoadBinProtein>()
            .Property(e => e.Uid)
            .HasColumnName("UID");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(DbModelBuilder modelBuilder);
}
