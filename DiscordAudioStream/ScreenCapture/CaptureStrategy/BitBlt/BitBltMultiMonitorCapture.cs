using System;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class BitBltMultimonitorCapture : ICaptureSource
	{
		private readonly ICaptureSource capture;
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

		public Bitmap CaptureFrame()
		{
			return capture.CaptureFrame();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			capture.Dispose();
		}

		private Rectangle GetMonitorArea()
		{
			return virtualScreenRectangle;
		}
	}
}
