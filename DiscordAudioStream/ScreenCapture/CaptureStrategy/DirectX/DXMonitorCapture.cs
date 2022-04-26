using Composition.WindowsRuntimeHelpers;
using System;
using System.Drawing;
using System.Windows.Forms;
using Windows.Graphics.Capture;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class DXMonitorCapture : ICaptureSource
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

		public Bitmap CaptureFrame()
		{
			return dxCapture.CaptureFrame();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			dxCapture.Dispose();
		}
	}
}
