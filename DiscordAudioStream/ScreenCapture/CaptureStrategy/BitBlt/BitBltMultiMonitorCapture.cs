using System;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class BitBltMultimonitorCapture : CaptureSource
	{
		private readonly CaptureSource capture;
		private readonly Rectangle virtualScreenRectangle = SystemInformation.VirtualScreen;

		public BitBltMultimonitorCapture(bool captureCursor)
		{
			var bitBlt = new BitBltCapture();
			bitBlt.CaptureAreaRect += GetMonitorArea;

			if (captureCursor)
			{
				var paintCursor = new CursorPainter(bitBlt);
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
