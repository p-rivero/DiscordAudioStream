using System;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public abstract class CustomAreaCapture : CaptureSource
	{
		private readonly static AreaForm areaForm = new AreaForm();
		private static int customAreaCaptureCount = 0;

		protected CustomAreaCapture()
		{
			if (customAreaCaptureCount == 0)
			{
				InvokeOnUI(areaForm.Show);
			}
			customAreaCaptureCount++;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			
			customAreaCaptureCount--;
			if (customAreaCaptureCount == 0)
			{
				InvokeOnUI(areaForm.Hide);
			}
		}

		protected Rectangle GetCustomArea()
		{
			// Omit pixels of the red border
			const int BORDER = AreaForm.BORDER_WIDTH_PX;
			int left = areaForm.Left + BORDER;
			int top = areaForm.Top + BORDER;
			int width = areaForm.Width - 2 * BORDER;
			int height = areaForm.Height - 2 * BORDER;

			return new Rectangle(left, top, width, height);
		}
	}

}
