using System;
using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy;

public class PrintWindowCapture : WindowCapture
{
    private readonly CaptureSource capture;
    private readonly IntPtr windowHandle;

    public PrintWindowCapture(IntPtr hWnd, bool captureCursor)
    {
        windowHandle = hWnd;

        PrintWindowCore printWindow = new(hWnd);

        if (captureCursor)
        {
            CursorPainter paintCursor = new(printWindow);
            paintCursor.CaptureAreaRect += () => GetWindowArea(windowHandle);
            capture = paintCursor;
        }
        else
        {
            capture = printWindow;
        }
    }

    public override Bitmap CaptureFrame()
    {
        return capture.CaptureFrame();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        capture.Dispose();
    }
}
