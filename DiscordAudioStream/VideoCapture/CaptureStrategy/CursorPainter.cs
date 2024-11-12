using System.Drawing;
using System.Drawing.Imaging;

using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public class CursorPainter : CaptureSource
{
    public Func<Rectangle>? CaptureAreaRect { get; set; }

    private readonly CaptureSource source;

    private HCURSOR currentCursorHandle;
    private Bitmap? cursorBitmap;
    private Point cursorHotspot = Point.Empty;

    public CursorPainter(CaptureSource source)
    {
        this.source = source;
        Logger.Log($"Instantiating CursorPainter source (wrapping {source.GetType().Name})");
    }

    public override Bitmap? CaptureFrame()
    {
        if (CaptureAreaRect == null)
        {
            throw new ArgumentException("Attempting to paint cursor without setting CaptureAreaRect");
        }

        Bitmap? bmp = source.CaptureFrame();
        if (bmp == null)
        {
            return null;
        }

        return PaintCursor(bmp, CaptureAreaRect().Location);
    }

    public override bool ScaleWithGPU => source.ScaleWithGPU;

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            source.Dispose();
            cursorBitmap?.Dispose();
        }
    }

    private Bitmap PaintCursor(Bitmap src, Point originPos)
    {
        CURSORINFO pci = CURSORINFO.New();

        if (!PInvoke.GetCursorInfo(ref pci) || pci.flags != CURSORINFO_FLAGS.CURSOR_SHOWING)
        {
            // Could not get cursor info, or the cursor was hidden. Do not paint the cursor
            return src;
        }

        if (currentCursorHandle != pci.hCursor)
        {
            UpdateCursorBitmap(pci.hCursor);
            currentCursorHandle = pci.hCursor;
        }
        if (cursorBitmap == null)
        {
            return src;
        }

        // Screen coordinates where the cursor has to be drawn (compensate for hotspot)
        Point cursorPos = new(
            pci.ptScreenPos.X - cursorHotspot.X,
            pci.ptScreenPos.Y - cursorHotspot.Y
        );
        // Transform from screen coordinates (relative to main screen) to window coordinates (relative to captured area)
        cursorPos.X -= originPos.X;
        cursorPos.Y -= originPos.Y;

        // Draw the cursor only if it's inside the bounds
        if (cursorPos.X >= 0 && cursorPos.Y >= 0 && cursorPos.X <= src.Width && cursorPos.Y <= src.Height)
        {
            using Graphics g = Graphics.FromImage(src);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(cursorBitmap, cursorPos);
        }

        return src;
    }

    private void UpdateCursorBitmap(HCURSOR hCursor)
    {
        if (!PInvoke.GetIconInfo(hCursor, out ICONINFO iconInfo))
        {
            return;
        }
        cursorBitmap?.Dispose();
        cursorBitmap = BitmapFromCursor(iconInfo);
        cursorHotspot = new((int)iconInfo.xHotspot, (int)iconInfo.yHotspot);
        CleanUpIconInfo(iconInfo);
    }

    private static Bitmap? BitmapFromCursor(in ICONINFO iconInfo)
    {
        if (iconInfo.hbmColor.IsNull)
        {
            return null;
        }
        using Bitmap bmp = Image.FromHbitmap(iconInfo.hbmColor);
        // Move data pointer (bmData.Scan0) from bmp to dstBitmap
        BitmapData bmData = bmp.LockBits(
            new Rectangle(0, 0, bmp.Width, bmp.Height),
            ImageLockMode.ReadOnly,
            bmp.PixelFormat
        );
        using Bitmap dstBitmap = new(
            bmData.Width,
            bmData.Height,
            bmData.Stride,
            PixelFormat.Format32bppArgb,
            bmData.Scan0
        );
        bmp.UnlockBits(bmData);

        return new Bitmap(dstBitmap);
    }

    private static void CleanUpIconInfo(in ICONINFO iconInfo)
    {
        if (!iconInfo.hbmMask.IsNull)
        {
            PInvoke.DeleteObject(iconInfo.hbmMask).AssertSuccess("Could not delete cursor mask");
        }
        if (!iconInfo.hbmColor.IsNull)
        {
            PInvoke.DeleteObject(iconInfo.hbmColor).AssertSuccess("Could not delete cursor color bitmap");
        }
    }
}
