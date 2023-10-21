using System.Drawing;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy;

public class BitBltWindowCapture : WindowCapture
{
    private readonly CaptureSource capture;
    private readonly HWND windowHandle;

    public BitBltWindowCapture(HWND hWnd, bool captureCursor)
    {
        windowHandle = hWnd;
        BitBltCapture bitBlt = new() { ClearBackground = true };
        bitBlt.CaptureAreaRect += () => GetWindowArea(windowHandle);

        if (captureCursor)
        {
            CursorPainter paintCursor = new(bitBlt);
            paintCursor.CaptureAreaRect += () => GetWindowArea(windowHandle);
            capture = paintCursor;
        }
        else
        {
            capture = bitBlt;
        }

        SetWindowTopmost(windowHandle, true);
    }

    public override Bitmap? CaptureFrame()
    {
        return capture.CaptureFrame();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        capture.Dispose();

        SetWindowTopmost(windowHandle, false);
    }

    private static void SetWindowTopmost(HWND hWnd, bool bringToFront)
    {
        HWND insertAfter = bringToFront ? HWND.HWND_TOPMOST : HWND.HWND_NOTOPMOST;
        SET_WINDOW_POS_FLAGS flags = SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOSIZE;
        PInvoke.SetWindowPos(hWnd, insertAfter, 0, 0, 0, 0, flags).AssertSuccess("SetWindowPos failed");
    }
}
