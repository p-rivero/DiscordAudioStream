using System.Drawing;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy;

public class BitBltCapture : CaptureSource
{
    public Func<Rectangle>? CaptureAreaRect { get; set; }

    // Create DC and Bitmap objects for reuse
    private HDC hdcSrc;
    private readonly HDC hdcDest;
    private HBITMAP hBitmap;
    private Size bitmapSize;

    public BitBltCapture()
    {
        HWND desktopWindow = PInvoke.GetDesktopWindow().AssertNotNull("Failed to get desktop window handle");

        InvokeOnUI(() => hdcSrc = PInvoke.GetWindowDC(desktopWindow).AssertNotNull("Failed to get desktop window DC"));

        hdcDest = PInvoke.CreateCompatibleDC(hdcSrc).AssertNotNull("Failed to create compatible DC");
    }

    public override Bitmap CaptureFrame()
    {
        if (CaptureAreaRect == null)
        {
            throw new InvalidOperationException("Attempting to capture frame without setting CaptureAreaRect");
        }

        // Get the target area
        Rectangle area = CaptureAreaRect();

        // Create the bitmap only if it doesn't exist or if its size has changed
        if (hBitmap.IsNull || area.Width != bitmapSize.Width || area.Height != bitmapSize.Height)
        {
            if (!hBitmap.IsNull)
            {
                PInvoke.DeleteObject(hBitmap).AssertSuccess("Failed to delete old bitmap");
            }
            hBitmap = PInvoke.CreateCompatibleBitmap(hdcSrc, area.Width, area.Height).AssertNotNull("CreateCompatibleBitmap failed");
            bitmapSize = new(area.Width, area.Height);
            PInvoke.SelectObject(hdcDest, hBitmap).AssertSuccess("Failed to select bitmap into DC");
        }

        PInvoke.BitBlt(hdcDest, 0, 0, area.Width, area.Height, hdcSrc, area.X, area.Y, ROP_CODE.SRCCOPY).AssertSuccess("BitBlt failed");

        return Image.FromHbitmap(hBitmap, IntPtr.Zero);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (hBitmap != IntPtr.Zero)
        {
            PInvoke.DeleteObject(hBitmap).AssertSuccess("Failed to delete bitmap");
        }
        if (hdcDest != IntPtr.Zero)
        {
            PInvoke.DeleteDC(hdcDest).AssertSuccess("Failed to delete destination DC");
        }
        if (hdcSrc != IntPtr.Zero)
        {
            ((BOOL)PInvoke.ReleaseDC(PInvoke.GetDesktopWindow(), hdcSrc)).AssertSuccess("Failed to release source DC");
        }
    }
}
