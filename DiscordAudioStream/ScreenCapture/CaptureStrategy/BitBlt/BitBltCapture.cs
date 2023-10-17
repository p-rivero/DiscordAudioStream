using System;
using System.Drawing;
using System.Runtime.InteropServices;

using DLLs;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy;

public class BitBltCapture : CaptureSource
{
    public Func<Rectangle>? CaptureAreaRect { get; set; }

    // Create DC and Bitmap objects for reuse
    private IntPtr hdcSrc;
    private readonly IntPtr hdcDest;
    private IntPtr hBitmap;
    private Size bitmapSize;

    public BitBltCapture(IntPtr desktopWindow)
    {
        InvokeOnUI(() => hdcSrc = User32.GetWindowDC(desktopWindow));
        hdcDest = Gdi32.CreateCompatibleDC(hdcSrc);
    }

    public BitBltCapture() : this(User32.GetDesktopWindow()) { }

    public override Bitmap CaptureFrame()
    {
        if (CaptureAreaRect == null)
        {
            throw new InvalidOperationException("Attempting to capture frame without setting CaptureAreaRect");
        }

        // Get the target area
        Rectangle area = CaptureAreaRect();

        // Create the bitmap only if it doesn't exist or if its size has changed
        if (hBitmap == IntPtr.Zero || area.Width != bitmapSize.Width || area.Height != bitmapSize.Height)
        {
            if (hBitmap != IntPtr.Zero)
            {
                Gdi32.DeleteObject(hBitmap);
            }
            hBitmap = Gdi32.CreateCompatibleBitmap(hdcSrc, area.Width, area.Height);
            bitmapSize = new(area.Width, area.Height);
            int result = Gdi32.SelectObject(hdcDest, hBitmap);
            if (result is 0 or Gdi32.HGDI_ERROR)
            {
                throw new ExternalException("Failed to select target bitmap");
            }
        }

        Gdi32.BitBlt(hdcDest, 0, 0, area.Width, area.Height, hdcSrc, area.X, area.Y, Gdi32.RasterOps.SRCCOPY);

        return Image.FromHbitmap(hBitmap, IntPtr.Zero);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (hBitmap != IntPtr.Zero)
        {
            bool success = Gdi32.DeleteObject(hBitmap);
            if (!success)
            {
                throw new ExternalException("Failed to delete bitmap");
            }
        }
        if (hdcDest != IntPtr.Zero)
        {
            bool success = Gdi32.DeleteDC(hdcDest);
            if (!success)
            {
                throw new ExternalException("Failed to delete destination DC");
            }
        }
        if (hdcSrc != IntPtr.Zero)
        {
            bool success = User32.ReleaseDC(User32.GetDesktopWindow(), hdcSrc);
            if (!success)
            {
                throw new ExternalException("Failed to release source DC");
            }
        }
    }
}
