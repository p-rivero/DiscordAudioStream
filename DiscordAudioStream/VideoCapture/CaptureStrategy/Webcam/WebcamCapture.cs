using System.Drawing;

using AForge.Video.DirectShow;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public class WebcamCapture : CaptureSource
{
    private readonly VideoCaptureDevice captureDevice;
    private readonly object currentBitmapLock = new();
    private Bitmap? currentBitmap;

    private Bitmap? CurrentBitmap
    {
        get
        {
            lock (currentBitmapLock)
            {
                return currentBitmap?.Clone() as Bitmap;
            }
        }
        set
        {
            Bitmap? newBitmap = value?.Clone() as Bitmap;
            lock (currentBitmapLock)
            {
                currentBitmap?.Dispose();
                currentBitmap = newBitmap;
            }
        }
    }

    public WebcamCapture(string monkerString)
    {
        captureDevice = new(monkerString);
        VideoCapabilities[] vcs = captureDevice.VideoCapabilities;
        captureDevice.VideoResolution = vcs[0];
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
