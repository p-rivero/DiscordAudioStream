
using System.Diagnostics;
using System.Drawing;

namespace DiscordAudioStream.VideoCapture;

public class DrawThread
{
    public event Action<Bitmap>? PaintFrame;

    private readonly VideoCaptureManager captureSource;

    public DrawThread(VideoCaptureManager captureSource)
    {
        this.captureSource = captureSource;
    }

    public void Start()
    {
        Thread drawThread = new(DrawThreadRun) { IsBackground = true, Name = "Draw Thread" };
        drawThread.Start();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope",
        Justification = "The result of GetNextFrame should not be disposed until the next frame arrives")]
    private void DrawThreadRun()
    {
        int fps = Properties.Settings.Default.CaptureFramerate;
        Logger.EmptyLine();
        Logger.Log($"Creating Draw thread. Target framerate: {fps} FPS ({captureSource.CaptureIntervalMs} ms)");

        Stopwatch stopwatch = new();

        while (true)
        {
            stopwatch.Restart();
            try
            {
                Bitmap? next = VideoCaptureManager.GetNextFrame();

                // No new data, keep displaying last frame
                if (next == null)
                {
                    continue;
                }

                PaintFrame?.Invoke(next);
            }
            catch (InvalidOperationException)
            {
                Logger.Log("Form is closing, stop Draw thread.");
                return;
            }
            stopwatch.Stop();

            int wait = captureSource.CaptureIntervalMs - (int)stopwatch.ElapsedMilliseconds;
            if (wait > 0)
            {
                Thread.Sleep(wait);
            }
        }
    }
}
