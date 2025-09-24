// File: Imaging/ImageComposer.cs
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;


public enum AnchorX { Left, Center, Right }
public enum AnchorY { Top, Middle, Bottom }

public static class ImageComposer
{
    /// <summary>
    /// Overlays an image (from file) onto the upper-left of the base PNG.
    /// overlayScale is the fraction of base width (0.25 = 25% of base width).
    /// </summary>
    public static void OverlayImageTopLeft(
        string basePngPath,
        string overlayImagePath,
        string outputPngPath,
        double overlayScale = 0.25,
        int paddingPx = 8)
    {
        using var baseImg = Image.FromFile(basePngPath);
        using var overlay = Image.FromFile(overlayImagePath);
        OverlayCore(baseImg, overlay, outputPngPath, overlayScale, paddingPx);
    }

    /// <summary>
    /// Overlays an image (from stream) onto the upper-left of the base PNG.
    /// </summary>
    public static void OverlayImageTopLeft(
        string basePngPath,
        Stream overlayImageStream,
        string outputPngPath,
        double overlayScale = 0.25,
        int paddingPx = 8)
    {
        using var baseImg = Image.FromFile(basePngPath);
        using var overlay = Image.FromStream(overlayImageStream);
        OverlayCore(baseImg, overlay, outputPngPath, overlayScale, paddingPx);
    }

    /// <summary>
    /// Precise placement & size with optional custom width/height (no aspect enforcement).
    /// If custom size is not provided, uses overlayScale * base width and preserves aspect.
    /// </summary>
    private static void OverlayCore(
        Image baseImg,
        Image overlay,
        string outputPngPath,
        double overlayScale,
        int paddingPx,
        int? x = null,
        int? y = null,
        int? customWidth = null,
        int? customHeight = null)
    {
        // default scaling if custom width not provided
        int targetW = customWidth ?? (int)Math.Round(baseImg.Width * overlayScale);
        int targetH;

        if (customHeight.HasValue)
        {
            targetH = customHeight.Value;
        }
        else
        {
            // preserve aspect ratio
            var scale = (double)targetW / Math.Max(1, overlay.Width);
            targetH = (int)Math.Round(Math.Max(1, overlay.Height) * scale);
        }

        using var bmp = new Bitmap(baseImg.Width, baseImg.Height, PixelFormat.Format32bppArgb);
        using (var g = Graphics.FromImage(bmp))
        {
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // draw base
            g.DrawImage(baseImg, 0, 0, baseImg.Width, baseImg.Height);

            // use user-supplied coordinates or default padding
            int drawX = x ?? paddingPx;
            int drawY = y ?? paddingPx;

            // clamp width/height to at least 1 pixel
            targetW = Math.Max(1, targetW);
            targetH = Math.Max(1, targetH);

            // draw overlay at requested size and position
            g.DrawImage(overlay, new Rectangle(drawX, drawY, targetW, targetH));
        }

        Directory.CreateDirectory(Path.GetDirectoryName(outputPngPath)!);
        bmp.Save(outputPngPath, ImageFormat.Png);
    }

    /// <summary>
    /// Core that preserves aspect automatically when only width or height is provided.
    /// If neither is provided, defaults to 25% of base width.
    /// </summary>
    private static void OverlayCoreWithAspect(
        Image baseImg,
        Image overlay,
        string outputPngPath,
        int x,
        int y,
        int? width,
        int? height)
    {
        int targetW, targetH;

        if (width.HasValue && height.HasValue)
        {
            // Both provided: use as-is
            targetW = Math.Max(1, width.Value);
            targetH = Math.Max(1, height.Value);
        }
        else if (width.HasValue)
        {
            // Width only: preserve aspect ratio for height
            var scale = (double)Math.Max(1, width.Value) / Math.Max(1, overlay.Width);
            targetW = Math.Max(1, width.Value);
            targetH = (int)Math.Round(Math.Max(1, overlay.Height) * scale);
        }
        else if (height.HasValue)
        {
            // Height only: preserve aspect ratio for width
            var scale = (double)Math.Max(1, height.Value) / Math.Max(1, overlay.Height);
            targetH = Math.Max(1, height.Value);
            targetW = (int)Math.Round(Math.Max(1, overlay.Width) * scale);
        }
        else
        {
            // Neither provided → default to 25% of base width, preserve aspect
            targetW = (int)Math.Round(Math.Max(1, baseImg.Width) * 0.25);
            var scale = (double)targetW / Math.Max(1, overlay.Width);
            targetH = (int)Math.Round(Math.Max(1, overlay.Height) * scale);
        }

        using var bmp = new Bitmap(baseImg.Width, baseImg.Height, PixelFormat.Format32bppArgb);
        using (var g = Graphics.FromImage(bmp))
        {
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // draw base
            g.DrawImage(baseImg, 0, 0, baseImg.Width, baseImg.Height);

            // draw overlay at (x, y) with computed size
            g.DrawImage(overlay, new Rectangle(x, y, targetW, targetH));
        }

        Directory.CreateDirectory(Path.GetDirectoryName(outputPngPath)!);
        bmp.Save(outputPngPath, ImageFormat.Png);
    }

    // ===== Public aspect-preserving overlay APIs =====

    public static void OverlayImageCustom(
        string basePngPath,
        string overlayImagePath,
        string outputPngPath,
        int x,
        int y,
        int? width = null,
        int? height = null)
    {
        using var baseImg = Image.FromFile(basePngPath);
        using var overlay = Image.FromFile(overlayImagePath);
        OverlayCoreWithAspect(baseImg, overlay, outputPngPath, x, y, width, height);
    }

    public static void OverlayImageCustom(
        string basePngPath,
        Stream overlayImageStream,
        string outputPngPath,
        int x,
        int y,
        int? width = null,
        int? height = null)
    {
        using var baseImg = Image.FromFile(basePngPath);
        using var overlay = Image.FromStream(overlayImageStream);
        OverlayCoreWithAspect(baseImg, overlay, outputPngPath, x, y, width, height);
    }

    // === New overload: base and overlay as streams, write to file ===
    public static void OverlayImageCustom(
        Stream baseImageStream,
        Stream overlayImageStream,
        string outputPngPath,
        int x,
        int y,
        int? width = null,
        int? height = null)
    {
        using var baseImg = Image.FromStream(baseImageStream);
        using var overlay = Image.FromStream(overlayImageStream);
        OverlayCoreWithAspect(baseImg, overlay, outputPngPath, x, y, width, height);
    }

    // === New: base+overlay as streams, return PNG as MemoryStream ===
    public static MemoryStream OverlayImageCustomToStream(
        Stream baseImageStream,
        Stream overlayImageStream,
        int x,
        int y,
        int? width = null,
        int? height = null)
    {
        using var baseImg = Image.FromStream(baseImageStream);
        using var overlay = Image.FromStream(overlayImageStream);

        var ms = new MemoryStream();
        OverlayCoreWithAspectToStream(baseImg, overlay, ms, x, y, width, height);
        ms.Position = 0;
        return ms;
    }

    // Internal: write PNG directly to a provided output stream
    private static void OverlayCoreWithAspectToStream(
        Image baseImg,
        Image overlay,
        Stream outputStream,
        int x,
        int y,
        int? width,
        int? height)
    {
        int targetW, targetH;

        if (width.HasValue && height.HasValue)
        {
            targetW = Math.Max(1, width.Value);
            targetH = Math.Max(1, height.Value);
        }
        else if (width.HasValue)
        {
            var scale = (double)Math.Max(1, width.Value) / Math.Max(1, overlay.Width);
            targetW = Math.Max(1, width.Value);
            targetH = (int)Math.Round(Math.Max(1, overlay.Height) * scale);
        }
        else if (height.HasValue)
        {
            var scale = (double)Math.Max(1, height.Value) / Math.Max(1, overlay.Height);
            targetH = Math.Max(1, height.Value);
            targetW = (int)Math.Round(Math.Max(1, overlay.Width) * scale);
        }
        else
        {
            targetW = (int)Math.Round(Math.Max(1, baseImg.Width) * 0.25);
            var scale = (double)targetW / Math.Max(1, overlay.Width);
            targetH = (int)Math.Round(Math.Max(1, overlay.Height) * scale);
        }

        using var bmp = new Bitmap(baseImg.Width, baseImg.Height, PixelFormat.Format32bppArgb);
        using (var g = Graphics.FromImage(bmp))
        {
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            g.DrawImage(baseImg, 0, 0, baseImg.Width, baseImg.Height);
            g.DrawImage(overlay, new Rectangle(x, y, Math.Max(1, targetW), Math.Max(1, targetH)));
        }

        bmp.Save(outputStream, ImageFormat.Png);
    }


    // === Anchored & clamped overlay helpers ===
    private static void ComputeSizePreserveAspect(
        Image baseImg,
        Image overlay,
        int? width,
        int? height,
        out int targetW,
        out int targetH)
    {
        if (width.HasValue && height.HasValue)
        {
            targetW = Math.Max(1, width.Value);
            targetH = Math.Max(1, height.Value);
            return;
        }
        if (width.HasValue)
        {
            var scale = (double)Math.Max(1, width.Value) / Math.Max(1, overlay.Width);
            targetW = Math.Max(1, width.Value);
            targetH = (int)Math.Round(Math.Max(1, overlay.Height) * scale);
            return;
        }
        if (height.HasValue)
        {
            var scale = (double)Math.Max(1, height.Value) / Math.Max(1, overlay.Height);
            targetH = Math.Max(1, height.Value);
            targetW = (int)Math.Round(Math.Max(1, overlay.Width) * scale);
            return;
        }
        // default: 25% of base width
        targetW = (int)Math.Round(Math.Max(1, baseImg.Width) * 0.25);
        var s = (double)targetW / Math.Max(1, overlay.Width);
        targetH = (int)Math.Round(Math.Max(1, overlay.Height) * s);
    }

    private static void ClampAndScaleToBounds(
        int baseW, int baseH,
        ref int w, ref int h)
    {
        if (w <= 0) w = 1;
        if (h <= 0) h = 1;
        if (w <= baseW && h <= baseH) return;

        var scale = Math.Min((double)baseW / w, (double)baseH / h);
        if (scale < 1.0)
        {
            w = Math.Max(1, (int)Math.Floor(w * scale));
            h = Math.Max(1, (int)Math.Floor(h * scale));
        }
    }

    private static void ComputeAnchoredPosition(
        int baseW, int baseH,
        int targetW, int targetH,
        AnchorX anchorX, AnchorY anchorY,
        int marginX, int marginY,
        bool clampToBounds,
        out int x, out int y)
    {
        x = anchorX switch
        {
            AnchorX.Left => marginX,
            AnchorX.Center => (baseW - targetW) / 2,
            AnchorX.Right => baseW - targetW - marginX,
            _ => marginX
        };

        y = anchorY switch
        {
            AnchorY.Top => marginY,
            AnchorY.Middle => (baseH - targetH) / 2,
            AnchorY.Bottom => baseH - targetH - marginY,
            _ => marginY
        };

        if (clampToBounds)
        {
            x = Math.Max(0, Math.Min(x, baseW - targetW));
            y = Math.Max(0, Math.Min(y, baseH - targetH));
        }
    }

    public static void OverlayImageAnchored(
        string basePngPath,
        string overlayImagePath,
        string outputPngPath,
        AnchorX anchorX,
        AnchorY anchorY,
        int marginX = 0,
        int marginY = 0,
        int? width = null,
        int? height = null,
        bool clampToBounds = true)
    {
        using var baseImg = Image.FromFile(basePngPath);
        using var overlay = Image.FromFile(overlayImagePath);
        int w, h;
        ComputeSizePreserveAspect(baseImg, overlay, width, height, out w, out h);
        ClampAndScaleToBounds(baseImg.Width, baseImg.Height, ref w, ref h);
        ComputeAnchoredPosition(baseImg.Width, baseImg.Height, w, h, anchorX, anchorY, marginX, marginY, clampToBounds, out var x, out var y);

        using var bmp = new Bitmap(baseImg.Width, baseImg.Height, PixelFormat.Format32bppArgb);
        using (var g = Graphics.FromImage(bmp))
        {
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            g.DrawImage(baseImg, 0, 0, baseImg.Width, baseImg.Height);
            g.DrawImage(overlay, new Rectangle(x, y, w, h));
        }

        Directory.CreateDirectory(Path.GetDirectoryName(outputPngPath)!);
        bmp.Save(outputPngPath, ImageFormat.Png);
    }

    public static void OverlayImageAnchored(
        Stream baseImageStream,
        Stream overlayImageStream,
        string outputPngPath,
        AnchorX anchorX,
        AnchorY anchorY,
        int marginX = 0,
        int marginY = 0,
        int? width = null,
        int? height = null,
        bool clampToBounds = true)
    {
        using var baseImg = Image.FromStream(baseImageStream);
        using var overlay = Image.FromStream(overlayImageStream);
        int w, h;
        ComputeSizePreserveAspect(baseImg, overlay, width, height, out w, out h);
        ClampAndScaleToBounds(baseImg.Width, baseImg.Height, ref w, ref h);
        ComputeAnchoredPosition(baseImg.Width, baseImg.Height, w, h, anchorX, anchorY, marginX, marginY, clampToBounds, out var x, out var y);

        using var bmp = new Bitmap(baseImg.Width, baseImg.Height, PixelFormat.Format32bppArgb);
        using (var g = Graphics.FromImage(bmp))
        {
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            g.DrawImage(baseImg, 0, 0, baseImg.Width, baseImg.Height);
            g.DrawImage(overlay, new Rectangle(x, y, w, h));
        }

        Directory.CreateDirectory(Path.GetDirectoryName(outputPngPath)!);
        bmp.Save(outputPngPath, ImageFormat.Png);
    }

    public static MemoryStream OverlayImageAnchoredToStream(
        Stream baseImageStream,
        Stream overlayImageStream,
        AnchorX anchorX,
        AnchorY anchorY,
        int marginX = 0,
        int marginY = 0,
        int? width = null,
        int? height = null,
        bool clampToBounds = true)
    {
        using var baseImg = Image.FromStream(baseImageStream);
        using var overlay = Image.FromStream(overlayImageStream);
        int w, h;
        ComputeSizePreserveAspect(baseImg, overlay, width, height, out w, out h);
        ClampAndScaleToBounds(baseImg.Width, baseImg.Height, ref w, ref h);
        ComputeAnchoredPosition(baseImg.Width, baseImg.Height, w, h, anchorX, anchorY, marginX, marginY, clampToBounds, out var x, out var y);

        var ms = new MemoryStream();
        using (var bmp = new Bitmap(baseImg.Width, baseImg.Height, PixelFormat.Format32bppArgb))
        using (var g = Graphics.FromImage(bmp))
        {
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            g.DrawImage(baseImg, 0, 0, baseImg.Width, baseImg.Height);
            g.DrawImage(overlay, new Rectangle(x, y, w, h));

            bmp.Save(ms, ImageFormat.Png);
        }
        ms.Position = 0;
        return ms;
    }

}