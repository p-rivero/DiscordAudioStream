using System.Drawing;
using System.Runtime.InteropServices;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public abstract class WindowCapture : CaptureSource
{
    protected static Rectangle GetWindowArea(HWND windowHandle)
    {
        if (PInvoke.IsIconic(windowHandle))
        {
            return Rectangle.Empty;
        }

        // Get size of client area (don't use X and Y, these are relative to the WINDOW rect)
        Rectangle clientRect = GetClientRect(windowHandle);
        try
        {
            // Get frame size and position (generally more accurate than GetWindowRect)
            PInvoke.DwmGetWindowAttribute(windowHandle, DWMWINDOWATTRIBUTE.DWMWA_EXTENDED_FRAME_BOUNDS, out RECT frame)
                .AssertSuccess("Failed to get window frame");

            // Trim the black bar at the top when the window is maximized,
            // as well as the title bar for applications with a defined client area
            int yOffset = frame.Height - clientRect.Height;

            return new Rectangle(frame.left + 1, frame.top + yOffset, clientRect.Width, clientRect.Height);
        }
        catch (ExternalException)
        {
            // GetWindowRect is not always accurate when the window is maximized, but doesn't fail on Windows 7
            return GetWindowRect(windowHandle);
        }
    }

    private static Rectangle GetClientRect(HWND hWnd)
    {
        if (PInvoke.GetClientRect(hWnd, out RECT clientRect))
        {
            return clientRect;
        }
        throw new InvalidOperationException("Window was closed");
    }

    private static Rectangle GetWindowRect(HWND hWnd)
    {
        PInvoke.GetWindowRect(hWnd, out RECT windowRect).AssertSuccess("GetWindowRect failed");
        return windowRect;
    }
}
