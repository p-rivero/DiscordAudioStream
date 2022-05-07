using DLLs;
using System;
using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	internal class BitBltCapture : CaptureSource
	{
		public delegate Rectangle CaptureAreaRectDelegate();
		public CaptureAreaRectDelegate CaptureAreaRect;

		public override Bitmap CaptureFrame()
		{
			if (CaptureAreaRect == null)
			{
				throw new InvalidOperationException("Attempting to capture frame without setting CaptureAreaRect");
			}

			// Get the target area
			Rectangle area = CaptureAreaRect();

			// Capture screen
			IntPtr hdcSrc = User32.GetWindowDC(User32.GetDesktopWindow());
			IntPtr hdcDest = Gdi32.CreateCompatibleDC(hdcSrc);
			IntPtr hBitmap = Gdi32.CreateCompatibleBitmap(hdcSrc, area.Width, area.Height);
			Gdi32.SelectObject(hdcDest, hBitmap);
			Gdi32.BitBlt(hdcDest, 0, 0, area.Width, area.Height, hdcSrc, area.X, area.Y, Gdi32.RasterOps.SRCCOPY);

			Bitmap result = Image.FromHbitmap(hBitmap);

			// Cleanup
			User32.ReleaseDC(User32.GetDesktopWindow(), hdcSrc);
			Gdi32.DeleteDC(hdcDest);
			Gdi32.DeleteObject(hBitmap);

			return result;
		}
	}
}
