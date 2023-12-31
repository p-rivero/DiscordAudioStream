using System.Drawing;

using AForge.Video.DirectShow;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public class WebcamCapture : CaptureSource
{
    private readonly VideoCaptureDevice captureDevice;
    private Bitmap? currentBitmap;

    private Bitmap? CurrentBitmap
    {
        get => currentBitmap?.Clone() as Bitmap;
        set
        {
            using Bitmap? oldBitmap = currentBitmap;
            currentBitmap = value?.Clone() as Bitmap;
        }
    }

    public WebcamCapture(string monkerString)
    {
        captureDevice = new(monkerString);
        captureDevice.NewFrame += (s, e) => CurrentBitmap = e.Frame;
        captureDevice.Start();
    }

    public override Bitmap? CaptureFrame()
    {
        return CurrentBitmap;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            captureDevice.Stop();
            CurrentBitmap = null;
        }
    }
}
