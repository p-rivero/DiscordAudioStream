using System.Drawing;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.Storage.Xps;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

internal class PrintWindowCore : WindowCapture
{
    private readonly HWND windowHandle;
    private readonly bool isWindows8_1 = Environment.OSVersion.Version >= new Version(6, 3);

    public PrintWindowCore(HWND hWnd)
    {
        windowHandle = hWnd;
    }

    public override Bitmap? CaptureFrame()
    {
        Rectangle winArea = GetWindowArea(windowHandle);

        if (winArea.Size == Size.Empty)
        {
            return null;
        }

        Bitmap result = new(winArea.Width, winArea.Height);

        uint renderFullContent = isWindows8_1 ? PInvoke.PW_RENDERFULLCONTENT : 0;
        uint flags = (uint)PRINT_WINDOW_FLAGS.PW_CLIENTONLY | renderFullContent;

        using Graphics g = Graphics.FromImage(result);
        bool success = PInvoke.PrintWindow(windowHandle, (HDC)g.GetHdc(), (PRINT_WINDOW_FLAGS)flags);
        return success ? result : null;
    }
}
