using System;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class BitBltCustomAreaCapture : ICaptureSource
	{
		private readonly ICaptureSource capture;
		private static readonly AreaForm areaForm = new AreaForm();

		public BitBltCustomAreaCapture(bool captureCursor)
		{
			var bitBlt = new BitBltCapture();
			// Display red rectangle for selecting area
			areaForm.SetMaximumArea(SystemInformation.VirtualScreen);
			areaForm.Show();

			bitBlt.CaptureAreaRect += GetCustomArea;

			if (captureCursor)
			{
				var paintCursor = new CursorPainter(bitBlt);
				paintCursor.CaptureAreaRect += GetCustomArea;
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
			areaForm.Hide();
		}

		private Rectangle GetCustomArea()
		{
			// Omit pixels of the red border
			int left = areaForm.Left + 1;
			int top = areaForm.Top + 1;
			int width = areaForm.Width - 2;
			int height = areaForm.Height - 2;

			return new Rectangle(left, top, width, height);
		}
	}
}
