using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy;

public class BitBltCustomAreaCapture : CustomAreaCapture
{
    private readonly CaptureSource capture;

    public BitBltCustomAreaCapture(bool captureCursor)
    {
        BitBltCapture bitBlt = new();
        bitBlt.CaptureAreaRect += () => GetCustomArea(false);

        if (captureCursor)
        {
            CursorPainter paintCursor = new(bitBlt);
            paintCursor.CaptureAreaRect += () => GetCustomArea(false);
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
}
