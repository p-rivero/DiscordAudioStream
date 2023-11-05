
using System.Diagnostics;
using System.Drawing;

namespace DiscordAudioStream.VideoCapture;

public class DrawThread
{
    public event Action<Bitmap>? PaintFrame;
    public Func<Bitmap>? GetCurrentlyDisplayedFrame { get; set; }

    private readonly VideoCaptureManager captureSource;

    private readonly Stopwatch timeSinceLastFrame = new();
    private const int TIME_TO_MINIMIZED_WARNING_MS = 300;

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
                    NoNewContent();
                    continue;
                }

                PaintFrame?.Invoke(next);
                timeSinceLastFrame.Restart();
            }
            catch (ObjectDisposedException)
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

    private void NoNewContent()
    {
        if (!timeSinceLastFrame.IsRunning || timeSinceLastFrame.ElapsedMilliseconds < TIME_TO_MINIMIZED_WARNING_MS)
        {
            return;
        }

        if (GetCurrentlyDisplayedFrame != null)
        {
            Bitmap frame = (Bitmap)GetCurrentlyDisplayedFrame().Clone();
            DrawMinimizedWarning(frame);
            PaintFrame?.Invoke(frame);
        }
        timeSinceLastFrame.Stop();
    }

    private static void DrawMinimizedWarning(Bitmap frame)
    {
        using Graphics g = Graphics.FromImage(frame);
        FillBackground(g, Color.FromArgb(150, 0, 0, 0));
        DrawText(g, "Minimized window");
    }

    private static void FillBackground(Graphics g, Color color)
    {
        using Brush darkFade = new SolidBrush(color);
        g.FillRectangle(darkFade, g.VisibleClipBounds);
    }

    private static void DrawText(Graphics g, string text)
    {
        const float SIZE_FACTOR = 0.4f;
        float fontSize = SIZE_FACTOR * g.VisibleClipBounds.Width / text.Length;
        using Font font = new(SystemFonts.MessageBoxFont.Name, fontSize, FontStyle.Bold);

        SizeF textMeasure = g.MeasureString(text, font);
        float x = (g.VisibleClipBounds.Width - textMeasure.Width) / 2;
        float y = (g.VisibleClipBounds.Height - textMeasure.Height) / 2;

        g.DrawString(text, font, Brushes.White, x, y);
    }
}
