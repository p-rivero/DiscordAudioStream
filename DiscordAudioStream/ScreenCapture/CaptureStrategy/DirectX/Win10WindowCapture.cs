using System;
using System.Drawing;

using Composition.WindowsRuntimeHelpers;

using Windows.Graphics.Capture;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy;

public class Win10WindowCapture : WindowCapture
{
    private readonly Win10Capture winCapture;
    private readonly IntPtr windowHandle;

    public Win10WindowCapture(IntPtr hWnd, bool captureCursor)
    {
        windowHandle = hWnd;

        GraphicsCaptureItem item = CaptureHelper.CreateItemForWindow(windowHandle);
        winCapture = new Win10Capture(item, captureCursor);
    }

    public override Bitmap CaptureFrame()
    {
        Bitmap result = winCapture.CaptureFrame();
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
        winCapture.Dispose();
    }
}
