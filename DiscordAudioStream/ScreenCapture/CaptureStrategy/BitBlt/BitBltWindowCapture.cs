using DLLs;
using System;
using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class BitBltWindowCapture : WindowCapture
	{
		private readonly CaptureSource capture;
		private readonly IntPtr windowHandle;

		public BitBltWindowCapture(IntPtr hWnd, bool captureCursor)
		{
			windowHandle = hWnd;
			var bitBlt = new BitBltCapture();
			bitBlt.CaptureAreaRect += () => GetWindowArea(windowHandle);

			if (captureCursor)
			{
				var paintCursor = new CursorPainter(bitBlt);
				paintCursor.CaptureAreaRect += () => GetWindowArea(windowHandle);
				capture = paintCursor;
			}
			else
			{
				capture = bitBlt;
			}

			// This method cannot capture occluded windows, set selected process as topmost
			SetWindowTopmost(windowHandle, true);
		}

		public override Bitmap CaptureFrame()
		{
			return capture.CaptureFrame();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			capture.Dispose();

			// We are no longer capturing the window, do not bring it to front
			SetWindowTopmost(windowHandle, false);
		}

		private static void SetWindowTopmost(IntPtr hWnd, bool bringToFront)
		{
			IntPtr insertAfter = bringToFront ? User32.HWND_TOPMOST : User32.HWND_NOTOPMOST;
			User32.SetWindowPos(hWnd, insertAfter, 0, 0, 0, 0, User32.SWP_NOMOVE | User32.SWP_NOSIZE);
		}
	}
}
