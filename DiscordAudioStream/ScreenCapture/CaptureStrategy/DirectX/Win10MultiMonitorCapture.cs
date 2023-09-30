using Windows.Foundation.Metadata;
using Composition.WindowsRuntimeHelpers;
using System;
using System.Drawing;
using System.Windows.Forms;
using Windows.Graphics.Capture;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class Win10MultiMonitorCapture : CaptureSource
	{

		private readonly Win10Capture winCapture;

		public static bool IsAvailable()
		{
			// TODO: Check windows version
			return ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 9);
		}

		public Win10MultiMonitorCapture(bool captureCursor)
		{
			// Passing null pointer to IGraphicsCaptureItemInterop::CreateForMonitor will capture the entire desktop
			// Wasn't able to find documentation for this, but it works
			winCapture = new Win10Capture(CaptureHelper.CreateItemForMonitor(IntPtr.Zero), captureCursor);
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
