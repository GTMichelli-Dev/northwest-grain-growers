using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;


    public partial class SeedModel : DbContext
    {
        public SeedModel()
             : base("name=Seed_DataConnectionString")
        {
        }
    //public virtual List<TreatTotalsByDateRange> GetTreatmentTotalsByDateRange(DateTime start, DateTime end)
    //{
    //    var startDateParam = new SqlParameter("@StartDate", start);
    //    var endDateParam = new SqlParameter("@EndDate", end);

    //    return this.Database.SqlQuery<TreatTotalsByDateRange>("dbo.TreatmentTotalsByDateRange @StartDate, @EndDate", startDateParam, endDateParam).ToList();
    //}


    public virtual DbSet<SeedTicketInfo> SeedTicketInfoes { get; set; }

    public virtual DbQuery<TreatTotalsByDateRange> TreatmentTotals { get; set; }

    public virtual DbSet<Seed_Items> Seed_Items { get; set; }
        public virtual DbSet<Seed_Treatments> Seed_Treatments { get; set; }
        public virtual DbSet<Seed_Varieties> Seed_Varieties { get; set; }
        public virtual DbSet<SeedInvoiceItem> SeedInvoiceItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Seed_Items>()
                .Property(e => e.UOMCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Seed_Items>()
                .Property(e => e.Pure_Seed)
                .HasPrecision(5, 4);

            modelBuilder.Entity<Seed_Items>()
                .Property(e => e.Other_Crop)
                .HasPrecision(5, 4);

            modelBuilder.Entity<Seed_Items>()
                .Property(e => e.Inert_Matter)
                .HasPrecision(5, 4);

            modelBuilder.Entity<Seed_Items>()
                .Property(e => e.Germination)
                .HasPrecision(5, 4);

            modelBuilder.Entity<Seed_Items>()
                .Property(e => e.Weed_Seed)
                .HasPrecision(5, 4);

            modelBuilder.Entity<Seed_Treatments>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Seed_Treatments>()
                .Property(e => e.DefaultValue)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Seed_Varieties>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Seed_Varieties>()
                .Property(e => e.DefaultValue)
                .HasPrecision(18, 4);

            modelBuilder.Entity<SeedInvoiceItem>()
                .Property(e => e.Invoice)
                .IsUnicode(false);

            modelBuilder.Entity<SeedInvoiceItem>()
                .Property(e => e.Quantity)
                .HasPrecision(13, 4);
        }
    }
