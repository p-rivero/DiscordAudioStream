using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;

using DiscordAudioStream.VideoCapture.CaptureStrategy;

namespace DiscordAudioStream.VideoCapture;

public class VideoCaptureManager : IDisposable
{
    public event Action? CaptureAborted;

    // Control framerate limit
    public int CaptureIntervalMs { get; private set; }

    // Number of frames that are stored in the queue
    private const int LIMIT_QUEUE_SZ = 3;

    private readonly Thread captureThread;
    private readonly CancellationTokenSource captureThreadToken = new();

    private readonly CaptureState captureState;
    private static readonly ConcurrentQueue<Bitmap> frameQueue = new();

    private CaptureSource? currentSource;
    private readonly object currentSourceLock = new();

    public VideoCaptureManager(CaptureState captureState)
    {
        this.captureState = captureState;
        captureState.StateChangeEventEnabled = true;
        // Update the capture state in a separate thread to avoid deadlocks
        captureState.StateChanged += () => new Thread(UpdateState).Start();

        // Start capture
        new Thread(UpdateState).Start();
        RefreshFramerate();
        captureThread = CreateCaptureThread();
        captureThread.Start();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        captureThreadToken.Cancel();
        captureThread.Join();
        currentSource?.Dispose();
    }

    // Return the next frame, if it exists (null otherwise)
    public static Bitmap? GetNextFrame()
    {
        if (frameQueue.TryDequeue(out Bitmap frame))
        {
            return frame;
        }
        return null;
    }

    public void RefreshFramerate()
    {
        CaptureIntervalMs = 1000 / Properties.Settings.Default.CaptureFramerate;
    }

    private Thread CreateCaptureThread()
    {
        return new(CaptureThreadRun) { IsBackground = true, Name = "Capture Thread" };
    }

    private void CaptureThreadRun()
    {
        int fps = Properties.Settings.Default.CaptureFramerate;
        Logger.EmptyLine();
        Logger.Log($"Creating Capture thread. Target framerate: {fps} FPS ({CaptureIntervalMs} ms)");
        Stopwatch stopwatch = new();

        while (currentSource == null)
        {
            Logger.Log("Capture thread waiting for capture source to initialize...");
            Thread.Sleep(100);
        }

        while (true)
        {
            stopwatch.Restart();
            try
            {
                lock (currentSourceLock)
                {
                    EnqueueFrame(currentSource.CaptureFrame());
                }
            }
            catch (Exception e)
            {
                Logger.EmptyLine();
                Logger.Log("Aborting capture due to exception.");
                Logger.Log(e);
                InvokeOnUI.RunSync(() => CaptureAborted?.Invoke());
            }
            stopwatch.Stop();

            int wait = CaptureIntervalMs - (int)stopwatch.ElapsedMilliseconds;
            if (wait > 0)
            {
                Thread.Sleep(wait);
            }

            if (captureThreadToken.IsCancellationRequested)
            {
                Logger.Log("Capture thread got cancellation request, stopping.");
                break;
            }
        }
    }

    private void UpdateState()
    {
        try
        {
            lock (currentSourceLock)
            {
                // Don't dispose the old source until the new source is instantiated without errors
                CaptureSource? oldSource = currentSource;
                currentSource = CaptureSourceFactory.Build(captureState);
                oldSource?.Dispose();
            }
            Logger.Log("Changed current source to " + currentSource.GetType().Name);
        }
        catch (Exception e)
        {
            if (currentSource != null)
            {
                Logger.EmptyLine();
                Logger.Log($"CANNOT INSTANTIATE NEW SOURCE. Returning to old source ({currentSource.GetType().Name}).");
                Logger.Log(e);

                // We already have a valid source, do not call Dispose and just show warning
                ShowMessage.Error()
                    .Text("Unable to display this item.")
                    .Text("If the problem persists, consider changing the capture method in Settings > Capture")
                    .Show();
                return;
            }

            Logger.EmptyLine();
            Logger.Log("CANNOT INSTANTIATE FIRST SOURCE. Changing capture methods to BitBlt.");
            Logger.Log(e);
            // We do not have a valid source, fallback to the safest methods
            captureState.ScreenMethod = CaptureState.ScreenCaptureMethod.BitBlt;
            captureState.WindowMethod = CaptureState.WindowCaptureMethod.BitBlt;
        }
    }

    private static void EnqueueFrame(Bitmap? frame)
    {
        // If there is no new content, avoid overwriting good frames
        if (frame == null)
        {
            return;
        }
        frameQueue.Enqueue(frame);

        if (frameQueue.Count > LIMIT_QUEUE_SZ && frameQueue.TryDequeue(out Bitmap b))
        {
            b.Dispose();
        }
    }
}
