using System;
using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class PrintWindowCapture : WindowCapture
	{
		private ICaptureSource capture;
		private IntPtr windowHandle;

		public PrintWindowCapture(IntPtr hWnd, bool captureCursor)
		{
			windowHandle = hWnd;

			var printWindow = new PrintWindowCore(hWnd);

			if (captureCursor)
			{
				var paintCursor = new CursorPainter(printWindow);
				paintCursor.CaptureAreaRect += GetWindowArea;
				capture = paintCursor;
			}
			else
			{
				capture = printWindow;
			}
		}

		public override Bitmap CaptureFrame()
		{
			return capture.CaptureFrame();
		}

		public override void Dispose()
		{
			capture.Dispose();
		}

		private Rectangle GetWindowArea()
		{
			return WindowCapture.GetWindowArea(windowHandle);
		}
	}
}
