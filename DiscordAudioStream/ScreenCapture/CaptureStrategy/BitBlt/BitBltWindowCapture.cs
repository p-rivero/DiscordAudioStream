using System;
using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class BitBltWindowCapture : WindowCapture
	{
		private readonly ICaptureSource capture;
		private readonly IntPtr windowHandle;

		public BitBltWindowCapture(IntPtr hWnd, bool captureCursor)
		{
			windowHandle = hWnd;
			var bitBlt = new BitBltCapture();
			bitBlt.CaptureAreaRect += GetWindowArea;

			if (captureCursor)
			{
				var paintCursor = new CursorPainter(bitBlt);
				paintCursor.CaptureAreaRect += GetWindowArea;
				capture = paintCursor;
			}
			else
			{
				capture = bitBlt;
			}
		}

		public override Bitmap CaptureFrame()
		{
			return capture.CaptureFrame();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			capture.Dispose();
		}

		private Rectangle GetWindowArea()
		{
			return WindowCapture.GetWindowArea(windowHandle);
		}
	}
}
