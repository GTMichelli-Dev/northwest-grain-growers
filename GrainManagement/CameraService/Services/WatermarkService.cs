using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace CameraService.Services;

/// <summary>
/// Burns audit info into a captured JPEG: load number, direction, and
/// the capture timestamp on a semi-transparent black band along the
/// bottom of the frame. Visible in any image viewer, can't be stripped
/// by a rename, and travels through downstream uploads / mirrors.
///
/// Failures (no font available, image format unreadable, etc.) fall back
/// to the original bytes so a broken watermark never blocks the capture
/// pipeline.
/// </summary>
public sealed class WatermarkService
{
    private readonly ILogger<WatermarkService> _logger;
    private readonly Font? _font;

    public WatermarkService(ILogger<WatermarkService> logger)
    {
        _logger = logger;
        _font = ResolveFont();
    }

    /// <summary>
    /// Adds the audit banner. Returns the original bytes unchanged on any
    /// error — never throws to the caller.
    /// </summary>
    public byte[] Apply(byte[] imageBytes, string ticket, string direction, DateTime capturedAtLocal)
    {
        if (imageBytes is null || imageBytes.Length == 0) return imageBytes ?? Array.Empty<byte>();
        if (_font is null) return imageBytes;

        try
        {
            using var image = Image.Load<Rgba32>(imageBytes);

            // Band height = 7% of the image, capped to a sensible range so
            // the text stays legible on tall thumbnails AND high-res frames.
            int bandHeight = Math.Clamp((int)(image.Height * 0.07), 36, 96);
            int bandTop = image.Height - bandHeight;

            // Font size derived from band height — keeps proportional with
            // image resolution so a 4K frame doesn't end up with a tiny banner.
            float fontSize = bandHeight * 0.45f;
            var bandFont   = new Font(_font, fontSize, FontStyle.Bold);

            // Three lines of info on one row — load, direction, time.
            var dirLabel = direction.ToUpperInvariant() switch
            {
                "IN"  => "INBOUND",
                "OUT" => "OUTBOUND",
                "BOL" => "BOL",
                _     => direction.ToUpperInvariant()
            };
            string left   = $"LOAD {ticket}";
            string middle = dirLabel;
            string right  = capturedAtLocal.ToString("yyyy-MM-dd HH:mm:ss");

            image.Mutate(ctx =>
            {
                // Semi-transparent black band
                ctx.Fill(Color.FromRgba(0, 0, 0, 170),
                         new RectangleF(0, bandTop, image.Width, bandHeight));

                float padX = bandHeight * 0.4f;
                float midY = bandTop + bandHeight / 2f;

                // Use TextOptions with horizontal+vertical alignment so we
                // don't have to measure each string manually. Origin acts
                // as the anchor point that HorizontalAlignment / Vertical
                // Alignment positions text around.
                ctx.DrawText(new RichTextOptions(bandFont)
                {
                    Origin = new PointF(padX, midY),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment   = VerticalAlignment.Center,
                }, left, Color.White);

                ctx.DrawText(new RichTextOptions(bandFont)
                {
                    Origin = new PointF(image.Width / 2f, midY),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment   = VerticalAlignment.Center,
                }, middle, Color.White);

                ctx.DrawText(new RichTextOptions(bandFont)
                {
                    Origin = new PointF(image.Width - padX, midY),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment   = VerticalAlignment.Center,
                }, right, Color.White);
            });

            using var ms = new MemoryStream();
            image.Save(ms, new JpegEncoder { Quality = 88 });
            return ms.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Watermark failed for ticket {Ticket} ({Direction}); using raw bytes.",
                ticket, direction);
            return imageBytes;
        }
    }

    /// <summary>
    /// Picks a font that exists on the host. Tries the obvious sans-serif
    /// families first, then falls back to whatever is registered. Returns
    /// null when no fonts are installed, in which case watermarking is a
    /// no-op (better than crashing the capture).
    /// </summary>
    private Font? ResolveFont()
    {
        // Preference order — Arial on Windows, DejaVu / Liberation on Pi /
        // Debian, Noto / DroidSans as broader Linux fallbacks.
        string[] preferred = {
            "Arial",
            "Helvetica",
            "DejaVu Sans",
            "Liberation Sans",
            "Noto Sans",
            "DroidSans",
        };

        foreach (var name in preferred)
        {
            if (SystemFonts.Collection.TryGet(name, out var family))
                return new Font(family, 16f, FontStyle.Bold);
        }

        var any = SystemFonts.Families.FirstOrDefault();
        if (any.Name is not null)
        {
            _logger.LogInformation("Watermark fonts: preferred families missing, using {Family}.", any.Name);
            return new Font(any, 16f, FontStyle.Bold);
        }

        _logger.LogWarning("No system fonts found. Watermarking will be skipped.");
        return null;
    }
}
