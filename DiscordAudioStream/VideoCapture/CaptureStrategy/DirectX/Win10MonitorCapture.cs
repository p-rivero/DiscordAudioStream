using System.Drawing;
using System.Windows.Forms;

using Composition.WindowsRuntimeHelpers;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public class Win10MonitorCapture : MonitorCapture
{
    private readonly Win10Capture winCapture;

    public Win10MonitorCapture(Screen monitor, bool captureCursor, bool hideTaskbar) : base(monitor, hideTaskbar)
    {
        winCapture = new(CaptureHelper.CreateItemForMonitor(MonitorHandle), captureCursor);

        if (hideTaskbar)
        {
            winCapture.CustomAreaCrop += GetWorkingAreaCrop;
        }
    }

    public override Bitmap? CaptureFrame()
    {
        return winCapture.CaptureFrame();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            winCapture.Dispose();
        }
    }
}
