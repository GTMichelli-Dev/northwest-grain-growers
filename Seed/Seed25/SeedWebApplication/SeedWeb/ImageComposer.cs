// File: Imaging/ImageComposer.cs
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

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

    private static void OverlayCore(
        Image baseImg,
        Image overlay,
        string outputPngPath,
        double overlayScale,
        int paddingPx)
    {
        var targetOverlayWidth = (int)Math.Round(baseImg.Width * overlayScale);
        var (newW, newH) = ResizeToFitWidth(overlay.Width, overlay.Height, targetOverlayWidth);

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

            // draw overlay in the upper-left with padding
            var x = paddingPx;
            var y = paddingPx;
            g.DrawImage(overlay, new Rectangle(x, y, newW, newH));
        }

        Directory.CreateDirectory(Path.GetDirectoryName(outputPngPath)!);
        bmp.Save(outputPngPath, ImageFormat.Png);
    }

    private static (int w, int h) ResizeToFitWidth(int origW, int origH, int targetW)
    {
        if (origW == 0 || origH == 0) return (targetW, targetW);
        var scale = (double)targetW / origW;
        var targetH = (int)Math.Round(origH * scale);
        return (Math.Max(1, targetW), Math.Max(1, targetH));
    }

    public static void AppendImageOnLeft(
    string basePngPath,
    string leftImagePath,
    string outputPngPath,
    int leftWidthPixels,          // fixed width for the left panel
    bool centerVertically = true)
    {
        using var baseImg = Image.FromFile(basePngPath);
        using var leftImg = Image.FromFile(leftImagePath);
        AppendLeftCore(baseImg, leftImg, outputPngPath, leftWidthPixels, centerVertically);
    }

    public static void AppendImageOnLeft(
        string basePngPath,
        Stream leftImageStream,
        string outputPngPath,
        int leftWidthPixels,
        bool centerVertically = true)
    {
        using var baseImg = Image.FromFile(basePngPath);
        using var leftImg = Image.FromStream(leftImageStream);
        AppendLeftCore(baseImg, leftImg, outputPngPath, leftWidthPixels, centerVertically);
    }

    private static void AppendLeftCore(
        Image baseImg,
        Image leftImg,
        string outputPngPath,
        int leftWidthPixels,
        bool centerVertically)
    {
        var (leftW, leftH) = ResizeToFitWidth(leftImg.Width, leftImg.Height, leftWidthPixels);

        var outW = leftW + baseImg.Width;
        var outH = Math.Max(leftH, baseImg.Height);

        using var bmp = new Bitmap(outW, outH, PixelFormat.Format32bppArgb);
        using (var g = Graphics.FromImage(bmp))
        {
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // background (optional): fill white
            g.Clear(Color.White);

            // draw left panel (x=0)
            var leftY = centerVertically ? (outH - leftH) / 2 : 0;
            g.DrawImage(leftImg, new Rectangle(0, leftY, leftW, leftH));

            // draw base image to the right of left panel
            var baseY = centerVertically ? (outH - baseImg.Height) / 2 : 0;
            g.DrawImage(baseImg, new Rectangle(leftW, baseY, baseImg.Width, baseImg.Height));
        }

        Directory.CreateDirectory(Path.GetDirectoryName(outputPngPath)!);
        bmp.Save(outputPngPath, ImageFormat.Png);
    }
}
