using Composition.WindowsRuntimeHelpers;
using System;
using System.Drawing;
using System.Threading.Tasks;
using Windows.Graphics.Capture;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class Win10WindowCapture : WindowCapture
	{
		private readonly Win10Capture winCapture;
		private readonly IntPtr windowHandle;

		public Win10WindowCapture(IntPtr hWnd, bool captureCursor)
		{
			windowHandle = hWnd;

			GraphicsCaptureItem item = CaptureHelper.CreateItemForWindow(windowHandle);
			winCapture = new Win10Capture(item, captureCursor);
		}

		public override Bitmap CaptureFrame()
		{
			Bitmap result = null;
			while (result == null)
			{
				// winCapture.CaptureFrame will block the thread until it gets a frame. 
				// If the operation does not succeed in 0.5 seconds, check if the window is still open
				Task task = Task.Run(() => result = winCapture.CaptureFrame());
				if (!task.Wait(TimeSpan.FromMilliseconds(500)))
				{
					// GetWindowArea will throw an exception if the window has been closed, but not if it's minimized
					WindowCapture.GetWindowArea(windowHandle);
				}
			}
			return result;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			winCapture.Dispose();
		}
	}
}
