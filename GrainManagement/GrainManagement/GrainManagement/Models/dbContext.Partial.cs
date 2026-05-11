#nullable disable
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.Models;

/// <summary>
/// Manually-maintained portion of <see cref="dbContext"/>. Holds DbSets
/// and fluent configuration for tables that have not been re-scaffolded
/// yet (e.g. <see cref="CameraAssignment"/>), so the auto-generated
/// dbContext.cs can be regenerated without losing additions.
/// </summary>
public partial class dbContext
{
    public virtual DbSet<CameraAssignment> CameraAssignments { get; set; }

    public virtual DbSet<TempWeightTicket> TempWeightTickets { get; set; }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CameraAssignment>(entity =>
        {
            entity.ToTable("CameraAssignments", "system");

            entity.HasKey(e => e.CameraAssignmentId);

            entity.Property(e => e.ServiceId).HasMaxLength(64).IsUnicode(false).IsRequired();
            entity.Property(e => e.CameraId).HasMaxLength(64).IsUnicode(false).IsRequired();
            entity.Property(e => e.DisplayName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Role).HasMaxLength(20).IsUnicode(false).IsRequired();

            entity.Property(e => e.IsPrimary).HasDefaultValue(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("sysutcdatetime()");

            entity.HasOne(e => e.Location)
                  .WithMany()
                  .HasForeignKey(e => e.LocationId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(e => new { e.LocationId, e.ScaleId, e.Role, e.IsActive })
                  .HasDatabaseName("IX_CamAssign_Lookup");
        });

        modelBuilder.Entity<TempWeightTicket>(entity =>
        {
            entity.ToTable("TempWeightTickets", "Inventory");

            entity.HasKey(e => e.TempTicketId);

            entity.Property(e => e.KioskId).HasMaxLength(64).IsUnicode(false).IsRequired();
            entity.Property(e => e.Units).HasMaxLength(8).IsUnicode(false).HasDefaultValue("lbs");
            entity.Property(e => e.ImagePath).HasMaxLength(500);
            entity.Property(e => e.Gross).HasColumnType("decimal(18,3)");
            entity.Property(e => e.Tare).HasColumnType("decimal(18,3)");
            entity.Property(e => e.Net).HasColumnType("decimal(18,3)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("sysutcdatetime()");

            entity.HasOne(e => e.Server)
                  .WithMany()
                  .HasForeignKey(e => e.ServerId)
                  .OnDelete(DeleteBehavior.NoAction)
                  .HasConstraintName("FK_TempTicket_Server");

            entity.HasIndex(e => new { e.ServerId, e.ScaleId, e.CreatedAt, e.CompletedAt })
                  .HasDatabaseName("IX_TempTicket_Picker");

            entity.HasIndex(e => e.CreatedAt)
                  .HasDatabaseName("IX_TempTicket_CreatedAt");
        });
    }
}
