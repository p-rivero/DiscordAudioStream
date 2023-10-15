using System;
using System.Drawing;

using DLLs;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy;

public abstract class WindowCapture : CaptureSource
{
    protected static Rectangle GetWindowArea(IntPtr windowHandle)
    {
        // Get size of client area (don't use X and Y, these are relative to the WINDOW rect)
        bool success = User32.GetClientRect(windowHandle, out User32.Rect clientRect);
        CheckSuccess(success);

        User32.Rect frame;
        try
        {
            // Get frame size and position (generally more accurate than GetWindowRect)
            frame = Dwmapi.GetRectAttr(windowHandle, Dwmapi.DwmWindowAttribute.EXTENDED_FRAME_BOUNDS);
        }
        catch (InvalidOperationException)
        {
            return GetWindowAreaFallback(windowHandle);
        }

        // Trim the black bar at the top when the window is maximized,
        // as well as the title bar for applications with a defined client area
        int yOffset = frame.Height - clientRect.Height;

        return new Rectangle(frame.left + 1, frame.top + yOffset, clientRect.Width, clientRect.Height);
    }

    private static Rectangle GetWindowAreaFallback(IntPtr windowHandle)
    {
        // GetWindowRect is not always accurate when the window is maximized, but doesn't fail on Windows 7
        bool success = User32.GetWindowRect(windowHandle, out User32.Rect windowRect);
        CheckSuccess(success);
        return new Rectangle(windowRect.left, windowRect.top, windowRect.Width, windowRect.Height);
    }

    private static void CheckSuccess(bool success)
    {
        if (!success)
        {
            throw new InvalidOperationException("Window was closed");
        }
    }
}
