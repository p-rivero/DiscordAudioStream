using System.Drawing;
using System.Windows.Forms;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public class DuplicationMonitorCapture : MonitorCapture
{
    private readonly CaptureSource source;

    public DuplicationMonitorCapture(Screen monitor, bool captureCursor, bool hideTaskbar) : base(monitor, hideTaskbar)
    {
        DuplicationCapture dupCapture = new(IndexOf(monitor));

        if (hideTaskbar)
        {
            dupCapture.CustomAreaCrop += GetWorkingAreaCrop;
        }

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

    public override bool ScaleWithGPU => source.ScaleWithGPU;

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            source.Dispose();
        }
    }

    private static int IndexOf(Screen screen)
    {
        return Array.FindIndex(
            DuplicationCapture.GPU0Adapter.Outputs,
            output => output.Description.DeviceName == screen.DeviceName
        );
    }
}
