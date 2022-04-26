using DLLs;
using System;
using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	internal class BitBltCapture : ICaptureSource
	{
		public delegate Rectangle CaptureAreaRectDelegate();
		public CaptureAreaRectDelegate CaptureAreaRect;

		public Bitmap CaptureFrame()
		{
			if (CaptureAreaRect == null)
			{
				throw new InvalidOperationException("Attempting to capture frame without setting CaptureAreaRect");
			}

			// Get the target area
			Rectangle area = CaptureAreaRect();

			// Capture screen
			IntPtr hdcSrc = User32.GetWindowDC(User32.GetDesktopWindow());
			IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
			IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, area.Width, area.Height);
			GDI32.SelectObject(hdcDest, hBitmap);
			GDI32.BitBlt(hdcDest, 0, 0, area.Width, area.Height, hdcSrc, area.X, area.Y, GDI32.RasterOps.SRCCOPY);

			Bitmap result = Image.FromHbitmap(hBitmap);

			// Cleanup
			User32.ReleaseDC(User32.GetDesktopWindow(), hdcSrc);
			GDI32.DeleteDC(hdcDest);
			GDI32.DeleteObject(hBitmap);

			return result;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			// Nothing to clean up
		}

	}
}
