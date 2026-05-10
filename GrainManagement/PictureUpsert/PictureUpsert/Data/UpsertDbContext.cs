using Microsoft.EntityFrameworkCore;
using PictureUpsert.Models;

namespace PictureUpsert.Data;

public sealed class UpsertDbContext : DbContext
{
    public UpsertDbContext(DbContextOptions<UpsertDbContext> options) : base(options) { }

    public DbSet<UpsertSettings> Settings => Set<UpsertSettings>();
    public DbSet<UpsertQueueItem> Queue   => Set<UpsertQueueItem>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<UpsertSettings>(e =>
        {
            e.ToTable("Settings");
            e.HasData(new UpsertSettings { Id = 1 });
        });

        mb.Entity<UpsertQueueItem>(e =>
        {
            e.ToTable("Queue");
            e.HasIndex(q => new { q.Status, q.EnqueuedUtc });
            e.HasIndex(q => q.FilePath).IsUnique();
        });
    }
}
