using Composition.WindowsRuntimeHelpers;
using System;
using System.Drawing;
using Windows.Graphics.Capture;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class DXWindowCapture : WindowCapture
	{
		private readonly DXCapture dxCapture;

		public DXWindowCapture(IntPtr hWnd, bool captureCursor)
		{
			GraphicsCaptureItem item = CaptureHelper.CreateItemForWindow(hWnd);
			dxCapture = new DXCapture(item, captureCursor);
		}

		public override Bitmap CaptureFrame()
		{
			return dxCapture.CaptureFrame();
		}

		public override void Dispose()
		{
			dxCapture.Dispose();
		}
	}
}
