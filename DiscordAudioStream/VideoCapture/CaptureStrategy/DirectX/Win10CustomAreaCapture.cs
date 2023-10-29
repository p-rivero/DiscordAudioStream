using System.Drawing;

using Composition.WindowsRuntimeHelpers;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public class Win10CustomAreaCapture : CustomAreaCapture
{
    private readonly Win10Capture capture;

    public Win10CustomAreaCapture(bool captureCursor)
    {
        capture = new(CaptureHelper.CreateItemForMonitor(CaptureHelper.ALL_SCREENS), captureCursor);
        capture.CustomAreaCrop += () => GetCustomArea(true);
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
