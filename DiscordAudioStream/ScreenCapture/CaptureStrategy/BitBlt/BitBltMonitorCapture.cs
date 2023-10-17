using System.Drawing;
using System.Windows.Forms;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy;

public class BitBltMonitorCapture : CaptureSource
{
    private readonly CaptureSource capture;
    private readonly Screen monitor;
    private readonly bool hideTaskbar;

    public BitBltMonitorCapture(Screen monitor, bool captureCursor, bool hideTaskbar)
    {
        this.monitor = monitor;
        this.hideTaskbar = hideTaskbar;

        BitBltCapture bitBlt = new();
        bitBlt.CaptureAreaRect += GetMonitorArea;

        if (captureCursor)
        {
            CursorPainter paintCursor = new(bitBlt);
            paintCursor.CaptureAreaRect += GetMonitorArea;
            capture = paintCursor;
        }
        else
        {
            capture = bitBlt;
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

    private Rectangle GetMonitorArea()
    {
        return hideTaskbar ? monitor.WorkingArea : monitor.Bounds;
    }
}
