using System;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class BitBltMultiMonitorCapture : CaptureSource
	{
		private readonly CaptureSource capture;
		private readonly Rectangle virtualScreenRectangle = SystemInformation.VirtualScreen;

		public BitBltMultiMonitorCapture(bool captureCursor)
		{
			BitBltCapture bitBlt = new BitBltCapture();
			bitBlt.CaptureAreaRect += GetMonitorArea;

			if (captureCursor)
			{
				CursorPainter paintCursor = new CursorPainter(bitBlt);
				paintCursor.CaptureAreaRect += GetMonitorArea;
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

		private Rectangle GetMonitorArea()
		{
			return virtualScreenRectangle;
		}
	}
}
