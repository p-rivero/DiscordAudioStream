using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy;

public class MeasureTime : CaptureSource
{
    private readonly CaptureSource capture;

    public MeasureTime(CaptureSource capture)
    {
        this.capture = capture;
        Logger.Log($"Instantiating MeasureTime source (wrapping {capture.GetType().Name})");
    }

    public override Bitmap? CaptureFrame()
    {
        System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
        Bitmap? bmp = capture.CaptureFrame();

        long elapsed_ms = watch.ElapsedMilliseconds;
        float fps = 1000f / elapsed_ms;
        Console.WriteLine($"{capture.GetType().Name}: {elapsed_ms} ms ({fps:0.#} FPS)");

        return bmp;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        capture.Dispose();
    }
}
