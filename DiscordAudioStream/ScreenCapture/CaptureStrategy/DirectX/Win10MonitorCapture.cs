using Composition.WindowsRuntimeHelpers;
using System;
using System.Drawing;
using System.Windows.Forms;
using Windows.Graphics.Capture;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class Win10MonitorCapture : CaptureSource
	{
		private readonly Win10Capture winCapture;

		public Win10MonitorCapture(Screen monitor, bool captureCursor)
		{
			// Get the handle of the screen
			// Screen.GetHashCode() is implemented as "return (int)hmonitor;"
			int handle = monitor.GetHashCode();
			IntPtr hMon = new IntPtr(handle);

			GraphicsCaptureItem item = CaptureHelper.CreateItemForMonitor(hMon);
			winCapture = new Win10Capture(item, captureCursor);
		}

		public override Bitmap CaptureFrame()
		{
			return winCapture.CaptureFrame();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			winCapture.Dispose();
		}
	}
}
