using DLLs;
using System;
using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public abstract class WindowCapture : CaptureSource
	{
		protected static Rectangle GetWindowArea(IntPtr windowHandle)
		{
			// Get size of client area (don't use X and Y, these are relative to the WINDOW rect)
			bool success = User32.GetClientRect(windowHandle, out User32.Rect clientRect);
			if (!success)
			{
				throw new InvalidOperationException("Window was closed");
			}

			// Get frame size and position (generally more accurate than GetWindowRect)
			User32.Rect frame = Dwmapi.GetRectAttr(windowHandle, Dwmapi.DwmWindowAttribute.EXTENDED_FRAME_BOUNDS);

			// Trim the black bar at the top when the window is maximized,
			// as well as the title bar for applications with a defined client area
			int yOffset = frame.Height - clientRect.Height;

			return new Rectangle(frame.X + 1, frame.Y + yOffset, clientRect.Width, clientRect.Height);
		}
	}
}
