using System;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class BitBltCustomAreaCapture : CaptureSource
	{
		private readonly CaptureSource capture;
		private static AreaForm areaForm = new AreaForm();
		// If true, disposing of a BitBltCustomAreaCapture will not hide the form
		private static bool disableHide = false;

		public BitBltCustomAreaCapture(bool captureCursor)
		{
			var bitBlt = new BitBltCapture();

			ShowAreaForm();

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

		public override Bitmap CaptureFrame()
		{
			return capture.CaptureFrame();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			capture.Dispose();
			HideAreaForm();
		}

		private static void ShowAreaForm()
		{
			try
			{
				// If the red rectangle was visible before creating the source, it should remain visible after
				if (areaForm.Visible) disableHide = true;
				areaForm.Show();
			}
			catch (ObjectDisposedException)
			{
				// This occurs when the AreaForm window is closed by another source. Create a new one.
				Logger.Log("Custom area window was killed. Creating a new one...");
				areaForm = new AreaForm();
				areaForm.Show();
			}
		}
		private static void HideAreaForm()
		{
			// Hide areaForm only if disableHide is false
			if (disableHide) disableHide = false;
			else areaForm.Hide();
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
