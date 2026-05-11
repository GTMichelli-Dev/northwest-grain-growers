namespace GrainManagement.Services.Images;

/// <summary>
/// Stitches per-camera ticket-image files into a single canonical
/// composite. Per-camera uploads land as
/// <c>{ticket}_{direction}__{cameraId}.jpg</c>; the composite is
/// the canonical <c>{ticket}_{direction}.jpg</c> the rest of the
/// app reads.
/// </summary>
public interface ITicketImageCompositor
{
    /// <summary>
    /// Build the composite for the given ticket + direction, reading
    /// every <c>{ticket}_{direction}__*.jpg</c> from the configured
    /// ticket-images folder. No-op if no per-camera files exist.
    /// </summary>
    Task CompositeAsync(string ticket, string direction, CancellationToken ct = default);
}
