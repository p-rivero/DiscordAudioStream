using DLLs;
using System;
using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class BitBltCapture : CaptureSource
	{
		public delegate Rectangle CaptureAreaRectDelegate();
		public CaptureAreaRectDelegate CaptureAreaRect { get; set; }

		// Create DC and Bitmap objects for reuse
		private readonly IntPtr hdcSrc;
		private readonly IntPtr hdcDest;
		private IntPtr hBitmap;
		private Size bitmapSize;

		public BitBltCapture()
		{
			// Initialize the DC and Bitmap objects in the constructor
			hdcSrc = User32.GetWindowDC(User32.GetDesktopWindow());
			hdcDest = Gdi32.CreateCompatibleDC(hdcSrc);
		}

		public override Bitmap CaptureFrame()
		{
			if (CaptureAreaRect == null)
			{
				throw new InvalidOperationException("Attempting to capture frame without setting CaptureAreaRect");
			}

			// Get the target area
			Rectangle area = CaptureAreaRect();

			// Create the bitmap only if it doesn't exist or if its size has changed
			if (hBitmap == IntPtr.Zero || area.Width != bitmapSize.Width || area.Height != bitmapSize.Height)
			{
				if (hBitmap != IntPtr.Zero)
				{
					Gdi32.DeleteObject(hBitmap);
				}
				hBitmap = Gdi32.CreateCompatibleBitmap(hdcSrc, area.Width, area.Height);
				bitmapSize = new Size(area.Width, area.Height);
				Gdi32.SelectObject(hdcDest, hBitmap);
			}

			Gdi32.BitBlt(hdcDest, 0, 0, area.Width, area.Height, hdcSrc, area.X, area.Y, Gdi32.RasterOps.SRCCOPY);

			return Image.FromHbitmap(hBitmap, IntPtr.Zero);
		}

		public new void Dispose()
		{
			if (hBitmap != IntPtr.Zero)
			{
				Gdi32.DeleteObject(hBitmap);
			}
			if (hdcDest != IntPtr.Zero)
			{
				Gdi32.DeleteDC(hdcDest);
			}
			if (hdcSrc != IntPtr.Zero)
			{
				User32.ReleaseDC(User32.GetDesktopWindow(), hdcSrc);
			}
		}
	}
}
