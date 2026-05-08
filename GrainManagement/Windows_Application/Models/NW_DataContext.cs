
using System;
using System.Collections.Generic;
using Microsoft.;



public partial class NW_DataContext : DbContext
{
    public NW_DataContext(DbContextOptions<NW_DataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<VwOpenTransferWeightSheet> VwOpenTransferWeightSheets { get; set; }

    public virtual DbSet<VwOpenWeightSheet> VwOpenWeightSheets { get; set; }

    public virtual DbSet<VwTransferWeightSheetInformation> VwTransferWeightSheetInformations { get; set; }

    public virtual DbSet<VwWeightSheetInformation> VwWeightSheetInformations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VwOpenTransferWeightSheet>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Open_Transfer_Weight_Sheets");

            entity.Property(e => e.CanAdd).HasColumnName("Can_Add");
            entity.Property(e => e.Carrier).HasMaxLength(50);
            entity.Property(e => e.Crop)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.LocationId).HasColumnName("Location_Id");
            entity.Property(e => e.MaxLoadsAllowed).HasColumnName("Max_Loads_Allowed");
            entity.Property(e => e.NotCompleted).HasColumnName("Not_Completed");
            entity.Property(e => e.SequenceId).HasColumnName("Sequence_ID");
            entity.Property(e => e.ServerName)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnName("Server_Name");
            entity.Property(e => e.Source)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.TotalLoads).HasColumnName("Total_Loads");
            entity.Property(e => e.WeightSheetUid).HasColumnName("Weight_Sheet_UID");
            entity.Property(e => e.WsId).HasColumnName("WS_Id");
        });

        modelBuilder.Entity<VwOpenWeightSheet>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Open_Weight_Sheets");

            entity.Property(e => e.BolType)
                .HasMaxLength(2)
                .HasColumnName("BOL_Type");
            entity.Property(e => e.BtnEdit)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("btnEDIT");
            entity.Property(e => e.CanAdd).HasColumnName("Can_Add");
            entity.Property(e => e.Carrier).HasMaxLength(50);
            entity.Property(e => e.CarrierId).HasColumnName("Carrier_Id");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.Crop)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.FsaNumber)
                .HasMaxLength(20)
                .HasColumnName("FSA_Number");
            entity.Property(e => e.IsLoadout).HasColumnName("Is_Loadout");
            entity.Property(e => e.Landlord).HasMaxLength(255);
            entity.Property(e => e.LocationId).HasColumnName("Location_Id");
            entity.Property(e => e.LotNumber).HasColumnName("Lot_Number");
            entity.Property(e => e.LotUid).HasColumnName("Lot_UID");
            entity.Property(e => e.MaxLoadsAllowed).HasColumnName("Max_Loads_Allowed");
            entity.Property(e => e.NotCompleted).HasColumnName("Not_Completed");
            entity.Property(e => e.ProducerDescription)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Producer_Description");
            entity.Property(e => e.Rate).HasColumnType("money");
            entity.Property(e => e.SequenceId).HasColumnName("Sequence_ID");
            entity.Property(e => e.ServerName)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnName("Server_Name");
            entity.Property(e => e.TotalLoads).HasColumnName("Total_Loads");
            entity.Property(e => e.TrainLoad)
                .IsRequired()
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Train_Load");
            entity.Property(e => e.Weighmaster)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.WeightSheetUid).HasColumnName("Weight_Sheet_UID");
            entity.Property(e => e.WsId).HasColumnName("WS_Id");
        });

        modelBuilder.Entity<VwTransferWeightSheetInformation>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwTransfer_Weight_Sheet_Information");

            entity.Property(e => e.BolType)
                .HasMaxLength(2)
                .HasColumnName("BOL_Type");
            entity.Property(e => e.CarrierDescription)
                .HasMaxLength(50)
                .HasColumnName("Carrier_Description");
            entity.Property(e => e.CarrierId).HasColumnName("Carrier_Id");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.CropDescription)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Crop_Description");
            entity.Property(e => e.CropId).HasColumnName("Crop_Id");
            entity.Property(e => e.IsLoadout).HasColumnName("Is_Loadout");
            entity.Property(e => e.LocationDescription)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Location_Description");
            entity.Property(e => e.LocationId).HasColumnName("Location_Id");
            entity.Property(e => e.OriginalPrinted).HasColumnName("Original_Printed");
            entity.Property(e => e.Rate).HasColumnType("money");
            entity.Property(e => e.SequenceId).HasColumnName("Sequence_ID");
            entity.Property(e => e.ServerName)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnName("Server_Name");
            entity.Property(e => e.Source)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.SourceId).HasColumnName("Source_Id");
            entity.Property(e => e.TotalBilled)
                .HasColumnType("money")
                .HasColumnName("Total_Billed");
            entity.Property(e => e.UomCode)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("UOM_Code");
            entity.Property(e => e.UomDescription)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName("UOM_Description");
            entity.Property(e => e.UomFactor).HasColumnName("UOM_Factor");
            entity.Property(e => e.VarietyDescription)
                .HasMaxLength(255)
                .HasColumnName("Variety_Description");
            entity.Property(e => e.Weighmaster)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.WeightSheetComment).HasColumnName("Weight_Sheet_Comment");
            entity.Property(e => e.WeightSheetUid).HasColumnName("Weight_Sheet_UID");
            entity.Property(e => e.WsId).HasColumnName("WS_Id");
        });

        modelBuilder.Entity<VwWeightSheetInformation>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwWeight_Sheet_Information");

            entity.Property(e => e.BolType)
                .HasMaxLength(2)
                .HasColumnName("BOL_Type");
            entity.Property(e => e.CarrierDescription)
                .HasMaxLength(50)
                .HasColumnName("Carrier_Description");
            entity.Property(e => e.CarrierId).HasColumnName("Carrier_Id");
            entity.Property(e => e.CloseDate).HasColumnName("Close_Date");
            entity.Property(e => e.CommodityCode)
                .HasMaxLength(539)
                .HasColumnName("Commodity_Code");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.CropDescription)
                .HasMaxLength(526)
                .HasColumnName("Crop_Description");
            entity.Property(e => e.CropId).HasColumnName("Crop_Id");
            entity.Property(e => e.CropVariety)
                .HasMaxLength(529)
                .HasColumnName("Crop_Variety");
            entity.Property(e => e.DateCreated)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Date_Created");
            entity.Property(e => e.FsaNumber)
                .HasMaxLength(20)
                .HasColumnName("FSA_Number");
            entity.Property(e => e.IsEndLot).HasColumnName("Is_End_Lot");
            entity.Property(e => e.IsLoadout).HasColumnName("Is_Loadout");
            entity.Property(e => e.IsNewLot).HasColumnName("Is_New_Lot");
            entity.Property(e => e.Landlord).HasMaxLength(255);
            entity.Property(e => e.LocationDescription)
                .HasMaxLength(50)
                .HasColumnName("Location_Description");
            entity.Property(e => e.LocationId).HasColumnName("Location_Id");
            entity.Property(e => e.LotClosed).HasColumnName("Lot_Closed");
            entity.Property(e => e.LotNumber).HasColumnName("Lot_Number");
            entity.Property(e => e.LotSampled).HasColumnName("Lot_Sampled");
            entity.Property(e => e.LotUid).HasColumnName("Lot_UID");
            entity.Property(e => e.NotCompleted).HasColumnName("Not_Completed");
            entity.Property(e => e.OriginalPrinted).HasColumnName("Original_Printed");
            entity.Property(e => e.ProducerDescription)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Producer_Description");
            entity.Property(e => e.ProducerId).HasColumnName("Producer_Id");
            entity.Property(e => e.ProducerIdDescription)
                .HasMaxLength(268)
                .HasColumnName("Producer_ID_Description");
            entity.Property(e => e.Rate).HasColumnType("money");
            entity.Property(e => e.SequenceId).HasColumnName("Sequence_ID");
            entity.Property(e => e.ServerName)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnName("Server_Name");
            entity.Property(e => e.SplitNumber).HasColumnName("Split_Number");
            entity.Property(e => e.StartDate).HasColumnName("Start_Date");
            entity.Property(e => e.TotalBilled)
                .HasColumnType("money")
                .HasColumnName("Total_Billed");
            entity.Property(e => e.TotalLoads).HasColumnName("Total_Loads");
            entity.Property(e => e.Uid).HasColumnName("UID");
            entity.Property(e => e.Variety).HasMaxLength(255);
            entity.Property(e => e.Weighmaster)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.WeightSheetClosed).HasColumnName("Weight_Sheet_Closed");
            entity.Property(e => e.WsId).HasColumnName("WS_Id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}