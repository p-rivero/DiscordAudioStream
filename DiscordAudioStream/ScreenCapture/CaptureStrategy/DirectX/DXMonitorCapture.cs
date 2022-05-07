using Composition.WindowsRuntimeHelpers;
using System;
using System.Drawing;
using System.Windows.Forms;
using Windows.Graphics.Capture;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class DXMonitorCapture : CaptureSource
	{
		private readonly DXCapture dxCapture;

		public DXMonitorCapture(Screen monitor, bool captureCursor)
		{
			// Get the handle of the screen
			// Screen.GetHashCode() is implemented as "return (int)hmonitor;"
			int handle = monitor.GetHashCode();
			IntPtr hMon = new IntPtr(handle);

			GraphicsCaptureItem item = CaptureHelper.CreateItemForMonitor(hMon);
			dxCapture = new DXCapture(item, captureCursor);
		}

		public override Bitmap CaptureFrame()
		{
			return dxCapture.CaptureFrame();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			dxCapture.Dispose();
		}
	}
}
