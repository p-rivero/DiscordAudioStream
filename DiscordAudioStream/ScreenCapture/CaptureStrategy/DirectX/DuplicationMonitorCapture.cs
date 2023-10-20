using System.Drawing;
using System.Windows.Forms;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy;

public class DuplicationMonitorCapture : CaptureSource
{
    private readonly CaptureSource source;
    private readonly Screen monitor;

    public DuplicationMonitorCapture(Screen monitor, bool captureCursor)
    {
        this.monitor = monitor;
        DuplicationCapture dupCapture = new(IndexOf(monitor));
        if (captureCursor)
        {
            CursorPainter paintCursor = new(dupCapture);
            paintCursor.CaptureAreaRect += GetMonitorArea;
            source = paintCursor;
        }
        else
        {
            source = dupCapture;
        }
    }

    public override Bitmap? CaptureFrame()
    {
        return source.CaptureFrame();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        source.Dispose();
    }

    private static int IndexOf(Screen screen)
    {
        return Array.FindIndex(
            DuplicationCapture.GPU0Adapter.Outputs,
            output => output.Description.DeviceName == screen.DeviceName
        );
    }

    private Rectangle GetMonitorArea()
    {
        return monitor.Bounds;
    }
}
