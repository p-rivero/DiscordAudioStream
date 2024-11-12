using System.Drawing;
using System.Windows.Forms;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public class BitBltMultiMonitorCapture : CaptureSource
{
    private readonly CaptureSource capture;
    private readonly Rectangle virtualScreenRectangle = SystemInformation.VirtualScreen;

    public BitBltMultiMonitorCapture(bool captureCursor)
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

    public override bool ScaleWithGPU => capture.ScaleWithGPU;

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            capture.Dispose();
        }
    }

    private Rectangle GetMonitorArea()
    {
        return virtualScreenRectangle;
    }
}
