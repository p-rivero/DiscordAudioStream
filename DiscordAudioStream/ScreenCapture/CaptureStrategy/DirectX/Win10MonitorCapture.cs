using System.Drawing;
using System.Windows.Forms;

using Composition.WindowsRuntimeHelpers;

using Windows.Win32.Graphics.Gdi;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy;

public class Win10MonitorCapture : CaptureSource
{
    private readonly Win10Capture winCapture;

    public Win10MonitorCapture(Screen monitor, bool captureCursor)
    {
        HMONITOR hMon = GetScreenHandle(monitor);
        winCapture = new(CaptureHelper.CreateItemForMonitor(hMon), captureCursor);
    }

    public override Bitmap? CaptureFrame()
    {
        return winCapture.CaptureFrame();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        winCapture.Dispose();
    }

    private static HMONITOR GetScreenHandle(Screen screen)
    {
        // Screen.GetHashCode() is implemented as "return (int)hmonitor"
        return (HMONITOR)(nint)screen.GetHashCode();
    }
}
