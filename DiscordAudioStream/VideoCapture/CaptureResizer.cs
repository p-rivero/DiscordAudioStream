using System.Drawing;

namespace DiscordAudioStream.VideoCapture;

// The order of the enum values must match the dropdown in the UI
public enum ScaleMode
{
    Fixed720p,
    Fixed1080p,
    Scale100,
    Scale50,
    Scale25,
    Scale20,
    Scale15,
    Scale10,
}

public static class CaptureResizer
{
    private const double DISCORD_ASPECT_RATIO = 16.0 / 9.0;
    private static double percentScaleFactor;
    private static uint fixedHeight;
    private static uint fixedWidth;

    static CaptureResizer()
    {
        SetScaleMode(ScaleMode.Scale100);
    }

    public static void SetScaleMode(ScaleMode mode)
    {
        switch (mode)
        {
            case ScaleMode.Fixed720p:
                percentScaleFactor = 0;
                fixedHeight = 720;
                fixedWidth = (uint)(fixedHeight * DISCORD_ASPECT_RATIO);
                break;
            case ScaleMode.Fixed1080p:
                percentScaleFactor = 0;
                fixedHeight = 1080;
                fixedWidth = (uint)(fixedHeight * DISCORD_ASPECT_RATIO);
                break;
            case ScaleMode.Scale100:
                percentScaleFactor = 1;
                break;
            case ScaleMode.Scale50:
                percentScaleFactor = Math.Sqrt(0.5);
                break;
            case ScaleMode.Scale25:
                percentScaleFactor = Math.Sqrt(0.25);
                break;
            case ScaleMode.Scale20:
                percentScaleFactor = Math.Sqrt(0.2);
                break;
            case ScaleMode.Scale15:
                percentScaleFactor = Math.Sqrt(0.15);
                break;
            case ScaleMode.Scale10:
                percentScaleFactor = Math.Sqrt(0.1);
                break;
            default:
                throw new ArgumentException("Unknown scale mode");
        }
    }

    public static bool ScaleWithGPU { get; set; }

    public static double GetScaleFactor(Size original)
    {
        return UsesPercentScaling ? percentScaleFactor : ComputeDynamicScaleFactor(original);
    }

    public static double GetCPUScaleFactor(Size original)
    {
        return ScaleWithGPU ? 1 : GetScaleFactor(original);
    }

    private static bool UsesPercentScaling => percentScaleFactor > 0.001;

    private static double ComputeDynamicScaleFactor(Size original)
    {
        double widthScaleFactor = (double)fixedWidth / original.Width;
        double heightScaleFactor = (double)fixedHeight / original.Height;

        double scaleFactor = Math.Min(widthScaleFactor, heightScaleFactor);
        // Do not upscale
        return Math.Min(1, scaleFactor);
    }
}

public static class ScaleExtensions
{
    public static Rectangle Scale(this Rectangle rect, double scaleFactor)
    {
        int newX = (int)(rect.X * scaleFactor);
        int newY = (int)(rect.Y * scaleFactor);
        int newWidth = (int)(rect.Width * scaleFactor);
        int newHeight = (int)(rect.Height * scaleFactor);
        return new Rectangle(newX, newY, newWidth, newHeight);
    }

    public static Size Scale(this Size size, double scaleFactor)
    {
        int newWidth = (int)(size.Width * scaleFactor);
        int newHeight = (int)(size.Height * scaleFactor);
        return new Size(newWidth, newHeight);
    }

    public static bool Is1(this double value)
    {
        return Math.Abs(value - 1) < 0.001;
    }
}
