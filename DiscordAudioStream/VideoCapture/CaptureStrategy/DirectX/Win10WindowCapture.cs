using System.Drawing;

using Composition.WindowsRuntimeHelpers;

using Windows.Graphics.Capture;
using Windows.Win32.Foundation;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public class Win10WindowCapture : WindowCapture
{
    private readonly Win10Capture winCapture;
    private readonly HWND windowHandle;

    public Win10WindowCapture(HWND hWnd, bool captureCursor)
    {
        windowHandle = hWnd;

        GraphicsCaptureItem item = CaptureHelper.CreateItemForWindow(windowHandle);
        winCapture = new(item, captureCursor);
    }

    public override Bitmap? CaptureFrame()
    {
        Bitmap? result = winCapture.CaptureFrame();
        if (result == null)
        {
            // Check if the window has been closed or it just doesn't have new content
            // GetWindowArea will throw an exception if the window has been closed, but not if it's minimized
            GetWindowArea(windowHandle);
        }
        return result;
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
