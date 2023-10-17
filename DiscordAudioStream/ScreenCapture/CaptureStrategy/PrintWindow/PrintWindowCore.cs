using System;
using System.Drawing;

using DLLs;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy;

internal class PrintWindowCore : WindowCapture
{
    private readonly IntPtr windowHandle;
    private readonly bool isWindows8_1OrHigher = Environment.OSVersion.Version >= new Version(6, 3);

    public PrintWindowCore(IntPtr hWnd)
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

        int renderFullContent = isWindows8_1OrHigher ? User32.PW_RENDERFULLCONTENT : 0;

        using (Graphics g = Graphics.FromImage(result))
        {
            User32.PrintWindow(windowHandle, g.GetHdc(), User32.PW_CLIENTONLY | renderFullContent);
        }
        return result;
    }
}
