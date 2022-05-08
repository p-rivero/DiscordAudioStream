using SharpDX.DXGI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class DuplicationMonitorCapture : CaptureSource
	{
		private readonly CaptureSource source;
		private readonly Screen monitor;

		public DuplicationMonitorCapture(Screen monitor, bool captureCursor)
		{
			this.monitor = monitor;
			DuplicationCapture dupCapture = new DuplicationCapture(IndexOf(monitor));
			if (captureCursor)
			{
				CursorPainter paintCursor = new CursorPainter(dupCapture);
				paintCursor.CaptureAreaRect += GetMonitorArea;
				source = paintCursor;
			}
			else
			{
				source = dupCapture;
			}
		}

		private int IndexOf(Screen screen)
		{
			var adapter = DuplicationCapture.Adapter;
			for (int i = 0; i < adapter.Outputs.Length; i++)
			{
				if (adapter.Outputs[i].Description.DeviceName == screen.DeviceName)
					return i;
			}
			throw new ArgumentException("Could not find index of screen");
		}

		public override Bitmap CaptureFrame()
		{
			return source.CaptureFrame();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			source.Dispose();
		}

		private Rectangle GetMonitorArea()
		{
			return monitor.Bounds;
		}
	}
}
