using System.Drawing;
using System.Windows.Forms;

using Windows.Win32.Graphics.Gdi;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public abstract class MonitorCapture : CaptureSource
{
    private readonly Screen monitor;
    private readonly bool hideTaskbar;

    protected MonitorCapture(Screen monitor, bool hideTaskbar)
    {
        this.monitor = monitor;
        this.hideTaskbar = hideTaskbar;
    }

    protected Rectangle GetMonitorArea()
    {
        return hideTaskbar ? monitor.WorkingArea : monitor.Bounds;
    }

    protected Rectangle GetWorkingAreaCrop()
    {
        Rectangle area = monitor.WorkingArea;
        // TODO: Remove the following 2 lines
        area.Height /= 2;
        area.Width /= 2;
        area.Location = Point.Empty;
        return area;
    }

    // Screen.GetHashCode() is implemented as "return (int)hmonitor"
    protected HMONITOR MonitorHandle => (HMONITOR)monitor.GetHashCode();
}
