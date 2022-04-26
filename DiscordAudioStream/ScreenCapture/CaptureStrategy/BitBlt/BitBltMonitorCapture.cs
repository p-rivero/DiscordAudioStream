using System;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class BitBltMonitorCapture : ICaptureSource
	{
		private readonly ICaptureSource capture;
		private readonly Screen monitor;
		private readonly bool hideTaskbar;

		public BitBltMonitorCapture(Screen monitor, bool captureCursor, bool hideTaskbar)
		{
			this.monitor = monitor;
			this.hideTaskbar = hideTaskbar;

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
			if (hideTaskbar)
			{
				return monitor.WorkingArea;
			}
			else
			{
				return monitor.Bounds;
			}
		}
	}
}
