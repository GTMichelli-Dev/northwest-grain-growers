using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace GrainManagement.Services.Images;

/// <summary>
/// SixLabors.ImageSharp-backed implementation. Horizontal strip of all
/// per-camera frames (resized to a common height), watermark band
/// across the bottom carrying load number + direction + timestamp.
/// </summary>
public sealed class TicketImageCompositor : ITicketImageCompositor
{
    // Target pane height. Frames get scaled to this so a 4K camera
    // doesn't blow up the composite, and a low-res webcam doesn't sit
    // tiny next to it.
    private const int PaneHeight = 720;

    // Watermark strip height at the bottom of the composite.
    private const int BandHeight = 64;

    // Padding for the watermark text inside the band.
    private const int BandPadX = 24;
    private const int BandPadY = 14;

    private readonly IConfiguration _config;
    private readonly ILogger<TicketImageCompositor> _log;

    public TicketImageCompositor(IConfiguration config, ILogger<TicketImageCompositor> log)
    {
        _config = config;
        _log = log;
    }

    public async Task CompositeAsync(string ticket, string direction, CancellationToken ct = default)
    {
        var imagesRoot = _config["TicketImages:PhysicalPath"];
        if (string.IsNullOrWhiteSpace(imagesRoot) || !Directory.Exists(imagesRoot))
        {
            _log.LogWarning("TicketImages:PhysicalPath is not configured or does not exist; skip composite.");
            return;
        }

        // Per-camera file pattern. The double underscore separates the
        // direction suffix from the camera id so the search glob is
        // unambiguous even when the load number itself contains
        // underscores.
        var pattern = $"{ticket}_{direction}__*.jpg";
        var files = Directory.EnumerateFiles(imagesRoot, pattern)
                             .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
                             .ToList();

        if (files.Count == 0)
        {
            _log.LogDebug("Composite skipped — no per-camera files match {Pattern}.", pattern);
            return;
        }

        var canonical = Path.Combine(imagesRoot, $"{ticket}_{direction}.jpg");

        // Single source — just copy. Cheaper than re-encoding, and the
        // CameraService has already burned its own watermark into the
        // frame.
        if (files.Count == 1)
        {
            try
            {
                File.Copy(files[0], canonical, overwrite: true);
                _log.LogInformation("Composite (single frame) for {Ticket} {Direction} → {Canonical}.",
                    ticket, direction, Path.GetFileName(canonical));
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "Composite copy failed for {Ticket} {Direction}.", ticket, direction);
            }
            return;
        }

        // Multiple sources — stitch horizontally + watermark band.
        var frames = new List<Image>();
        try
        {
            int totalWidth = 0;
            foreach (var f in files)
            {
                try
                {
                    var img = await Image.LoadAsync(f, ct);
                    if (img.Height != PaneHeight)
                    {
                        int w = (int)Math.Round(img.Width * (double)PaneHeight / img.Height);
                        img.Mutate(x => x.Resize(w, PaneHeight));
                    }
                    frames.Add(img);
                    totalWidth += img.Width;
                }
                catch (Exception ex)
                {
                    _log.LogWarning(ex, "Composite: skipping unreadable frame {File}.", Path.GetFileName(f));
                }
            }

            if (frames.Count == 0)
            {
                _log.LogWarning("Composite for {Ticket} {Direction}: no readable frames.", ticket, direction);
                return;
            }

            using var canvas = new Image<Rgba32>(totalWidth, PaneHeight + BandHeight);

            // Fill with the band's background color so the bottom strip is
            // ready before we overlay panes.
            canvas.Mutate(c => c.Fill(Color.Black));

            int x = 0;
            foreach (var f in frames)
            {
                canvas.Mutate(c => c.DrawImage(f, new Point(x, 0), 1f));
                x += f.Width;
            }

            // Watermark text — load number, direction label, local timestamp.
            // System fonts vary across hosts; SystemFonts.TryGet finds the
            // first available match so the composite never fails over a
            // missing font.
            var font = ResolveFont(28);
            if (font is not null)
            {
                var caption = $"Load {ticket}   ·   {DirectionLabel(direction)}   ·   {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                canvas.Mutate(c => c.DrawText(
                    caption, font, Color.White,
                    new PointF(BandPadX, PaneHeight + BandPadY)));
            }

            var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder { Quality = 88 };
            await canvas.SaveAsJpegAsync(canonical, encoder, ct);

            _log.LogInformation(
                "Composite for {Ticket} {Direction}: {Count} frames → {File} ({Width}x{Height}).",
                ticket, direction, frames.Count, Path.GetFileName(canonical),
                canvas.Width, canvas.Height);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Composite failed for {Ticket} {Direction}.", ticket, direction);
        }
        finally
        {
            foreach (var f in frames) f.Dispose();
        }
    }

    private static Font? ResolveFont(float size)
    {
        // First system font that exists wins. Windows Server hosts
        // typically have Arial / Verdana; Linux containers may only
        // have DejaVu Sans.
        foreach (var name in new[] { "Arial", "Helvetica", "Verdana", "DejaVu Sans", "Liberation Sans" })
        {
            if (SystemFonts.TryGet(name, out var family))
                return family.CreateFont(size, FontStyle.Bold);
        }
        // Last resort — the first system font of any kind.
        var any = SystemFonts.Collection.Families.FirstOrDefault();
        return any == default ? null : any.CreateFont(size, FontStyle.Bold);
    }

    private static string DirectionLabel(string direction) => direction.ToLowerInvariant() switch
    {
        "in"  or "inbound"  => "INBOUND",
        "out" or "outbound" => "OUTBOUND",
        "bol"               => "BOL",
        "tmp" or "tempticket" => "TEMP TICKET",
        _ => direction.ToUpperInvariant(),
    };
}
