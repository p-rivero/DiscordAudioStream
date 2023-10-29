using System.Drawing;

using Windows.Win32.Foundation;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public class PrintWindowCapture : WindowCapture
{
    private readonly CaptureSource capture;

    public PrintWindowCapture(HWND hWnd, bool captureCursor)
    {
        PrintWindowCore printWindow = new(hWnd);

        if (captureCursor)
        {
            CursorPainter paintCursor = new(printWindow);
            paintCursor.CaptureAreaRect += () => GetWindowArea(hWnd);
            capture = paintCursor;
        }
        else
        {
            capture = printWindow;
        }
    }

    public override Bitmap? CaptureFrame()
    {
        return capture.CaptureFrame();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        capture.Dispose();
    }
}
