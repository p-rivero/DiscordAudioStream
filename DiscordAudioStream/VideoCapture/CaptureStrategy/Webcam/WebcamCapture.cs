using System.Drawing;

using AForge.Video.DirectShow;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public class WebcamCapture : CaptureSource
{
    private VideoCaptureDevice captureDevice;
    private bool gotFrames;
    private readonly Thread captureMonitoringThread;
    private readonly CancellationTokenSource captureMonitoringCancel = new();

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
            gotFrames = true;
        }
    }

    public WebcamCapture(string monkerString)
    {
        captureDevice = InitializeCaptureDevice(monkerString);
        captureMonitoringThread = new(() => MonitorCaptureDevice(monkerString));
        captureMonitoringThread.Start();
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
            captureMonitoringCancel.Cancel();
            captureMonitoringThread.Join();
            captureMonitoringCancel.Dispose();
            captureDevice.Stop();
            CurrentBitmap = null;
        }
    }

    private VideoCaptureDevice InitializeCaptureDevice(string monkerString)
    {
        VideoCaptureDevice device = new(monkerString);
        VideoCapabilities[] vcs = device.VideoCapabilities;
        device.VideoResolution = vcs[0];
        device.NewFrame += (s, e) => CurrentBitmap = e.Frame;
        device.Start();
        return device;
    }

    private void MonitorCaptureDevice(string monkerString)
    {
        while (true)
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
                if (captureMonitoringCancel.IsCancellationRequested)
                {
                    return;
                }
            }
            if (!gotFrames)
            {
                Logger.Log("Webcam capture device failed to get frames, attempting to reinitialize");
                captureDevice.Stop();
                captureDevice = InitializeCaptureDevice(monkerString);
            }
            gotFrames = false;
        }
    }
}
