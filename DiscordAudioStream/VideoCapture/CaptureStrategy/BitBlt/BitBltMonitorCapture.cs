using System.Drawing;
using System.Windows.Forms;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public class BitBltMonitorCapture : MonitorCapture
{
    private readonly CaptureSource capture;

    public BitBltMonitorCapture(Screen monitor, bool captureCursor, bool hideTaskbar) : base(monitor, hideTaskbar)
    {
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
        if (disposing)
        {
            capture.Dispose();
        }
    }
}
