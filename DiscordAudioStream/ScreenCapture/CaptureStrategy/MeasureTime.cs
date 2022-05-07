using System;
using System.Drawing;

namespace DiscordAudioStream.ScreenCapture.CaptureStrategy
{
	public class MeasureTime : CaptureSource
	{
		private readonly CaptureSource capture;

		public MeasureTime(CaptureSource capture)
		{
			this.capture = capture;
		}

		public override Bitmap CaptureFrame()
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();
			Bitmap bmp = capture.CaptureFrame();
			Console.WriteLine("Capture using " + capture.GetType().Name + ": " + watch.ElapsedMilliseconds + " ms");
			return bmp;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			capture.Dispose();
		}
	}
}
